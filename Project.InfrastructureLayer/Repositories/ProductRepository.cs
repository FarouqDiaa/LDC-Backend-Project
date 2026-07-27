using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Project.InfrastructureLayer.Entities;
using Project.InfrastructureLayer.Abstractions;


namespace Project.InfrastructureLayer.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        public ProductRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            var cacheKey = $"Product-{id}";

            if (!_cache.TryGetValue(cacheKey, out Product product))
            {
                product = await _context.Products
                                        .Include(p => p.ProductImages)
                                        .FirstOrDefaultAsync(p => p.Id == id);

                if (product != null)
                {
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                        Size = 1
                    };
                    _cache.Set(cacheKey, product, cacheOptions);
                }
            }

            return product;
        }


        public async Task<Product> GetByNameAsync(string name)
        {
            var cacheKey = $"Product-{name}";
            if (!_cache.TryGetValue(cacheKey, out Product product))
            {
                product = await _context.Products.Where(p => p.Name == name)
                                                 .Include(p => p.ProductImages)
                                                 .FirstOrDefaultAsync();

                if (product != null)
                {
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                        Size = 1
                    };

                    _cache.Set(cacheKey, product, cacheOptions);
                }
            }

            return product;
        }

        public async Task<bool> IsProductExistsAsync(string name)
        {
            var cacheKey = $"ProductExists-{name}";
            if (!_cache.TryGetValue(cacheKey, out bool exists))
            {
                exists = await _context.Products
                                       .AsNoTracking()
                                       .AnyAsync(p => p.Name == name);

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    Size = 1
                };
                _cache.Set(cacheKey, exists, cacheOptions);
            }
            return exists;

        }


        private const int DefaultPageSize = 25;

        public async Task<IEnumerable<Product>> GetAllPagedAsync(int pageNumber, int pageSize, string? type = null)
        {
            var hasType = !string.IsNullOrWhiteSpace(type);
            // Only cache the unfiltered default page; filtered results are too varied.
            var cacheable = !hasType && pageNumber == 1 && pageSize == DefaultPageSize;
            var cacheKey = $"ProductsPage-{pageNumber}-{pageSize}";

            if (cacheable && _cache.TryGetValue(cacheKey, out IEnumerable<Product> cached))
            {
                return cached;
            }

            var query = _context.Products
                                .AsNoTracking()
                                .Include(p => p.ProductImages)
                                .Where(p => p.IsDeleted == false);

            if (hasType)
            {
                query = query.Where(p => p.Type == type);
            }

            var products = await query
                                     .OrderByDescending(p => p.CreatedOn)
                                     .Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToListAsync();

            if (cacheable)
            {
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60),
                    Size = 1
                };

                _cache.Set(cacheKey, products, cacheOptions);
            }

            return products;
        }

        public async Task<int> GetProductsCountAsync(string? type = null, bool includeDeleted = false)
        {
            var query = _context.Products.AsNoTracking();

            if (!includeDeleted)
            {
                query = query.Where(p => p.IsDeleted == false);
            }

            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(p => p.Type == type);
            }

            return await query.CountAsync();
        }

        /// <summary>
        /// Swaps a product's images for a new set. Existing rows are deleted rather
        /// than reassigned, so no orphans are left behind.
        /// </summary>
        public async Task ReplaceImagesAsync(Guid productId, IEnumerable<ProductImage> images)
        {
            var existing = await _context.ProductImages
                                         .Where(i => i.ProductId == productId)
                                         .ToListAsync();

            _context.ProductImages.RemoveRange(existing);

            foreach (var image in images)
            {
                image.Id = image.Id == Guid.Empty ? Guid.NewGuid() : image.Id;
                image.ProductimageId = Guid.NewGuid();
                image.ProductId = productId;
                image.CreatedOn = DateTime.UtcNow;
                image.UpdatedOn = DateTime.UtcNow;
            }

            await _context.ProductImages.AddRangeAsync(images);
            InvalidateStorefrontCaches();
        }

        // Storefront/home-page caches that must be dropped whenever the product
        // set changes, or newly added products stay hidden until their TTL lapses.
        private void InvalidateStorefrontCaches()
        {
            _cache.Remove($"ProductsPage-1-{DefaultPageSize}");
            _cache.Remove($"ProductsPageAdmin-1-{DefaultPageSize}");
            _cache.Remove("ProductCategories");
            // Best-sellers is keyed by count; clear the common sizes.
            for (var count = 1; count <= 20; count++)
            {
                _cache.Remove($"ProductsBestSellers-{count}");
            }
        }

        public async Task Update(Product product)
        {
            var existingProduct = await _context.Products
                                                .AsNoTracking()
                                                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (existingProduct.Name != product.Name)
            {
                _cache.Remove($"ProductExists-{existingProduct.Name}");
                _cache.Remove($"Product-{existingProduct.Name}");
            }

            // The by-id entry must go too, or a re-read returns the pre-update copy.
            _cache.Remove($"Product-{product.Id}");
            InvalidateStorefrontCaches();


            product.UpdatedOn = DateTime.UtcNow;

            _context.Products.Update(product);

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                Size = 1
            };
            _cache.Set($"ProductExists-{product.Name}", true, cacheOptions);
            _cache.Set($"Product-{product.Name}", product, cacheOptions);
        }


        public async Task AddAsync(Product product)
        {
            product.UpdatedOn = DateTime.UtcNow;
            product.CreatedOn = DateTime.UtcNow;
            await _context.Products.AddAsync(product);
            InvalidateStorefrontCaches();
        }

        public async Task RemoveByIdAsync(Guid id)
        {
            var product = await _context.Products
                                        .FirstOrDefaultAsync(p => p.Id == id);

            product.IsDeleted = true;
            product.UpdatedOn = DateTime.UtcNow;
            _cache.Remove($"ProductExists-{product.Name}");
            _cache.Remove($"Product-{product.Name}");
            _cache.Remove($"Product-{product.Id}");
            InvalidateStorefrontCaches();
        }



        public async Task<bool> IsProductExistsByIdAsync(Guid id)
        {
            var cacheKey = $"ProductExists-{id}";
            if (!_cache.TryGetValue(cacheKey, out bool exists))
            {
                exists = await _context.Products
                                       .AsNoTracking()
                                       .AnyAsync(p => p.Id == id);

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    Size = 1
                };
                _cache.Set(cacheKey, exists, cacheOptions);

            }

            return exists;
        }

        // ── Home-page storefront queries (public, non-deleted only) ────────

        public async Task<IEnumerable<Product>> GetBestSellersAsync(int count)
        {
            var cacheKey = $"ProductsBestSellers-{count}";
            if (_cache.TryGetValue(cacheKey, out IEnumerable<Product> cached))
            {
                return cached;
            }

            // Rank by total units sold across all order items.
            var productIds = await _context.OrderItems
                .AsNoTracking()
                .GroupBy(oi => oi.ProductId)
                .Select(g => new { ProductId = g.Key, Units = g.Sum(oi => oi.Quantity) })
                .OrderByDescending(x => x.Units)
                .Select(x => x.ProductId)
                .ToListAsync();

            var products = await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductImages)
                .Where(p => !p.IsDeleted && productIds.Contains(p.Id))
                .ToListAsync();

            // Preserve the sales ranking, which the DB query above doesn't keep.
            var rank = productIds.Select((id, index) => new { id, index })
                                 .ToDictionary(x => x.id, x => x.index);
            var ordered = products
                .OrderBy(p => rank[p.Id])
                .Take(count)
                .ToList();

            _cache.Set(cacheKey, ordered, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15),
                Size = 1
            });

            return ordered;
        }

        public async Task<IEnumerable<Product>> GetNewArrivalsAsync(int count)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductImages)
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.CreatedOn)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetLastPiecesAsync(int count, int maxStock)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductImages)
                .Where(p => !p.IsDeleted && p.StockQuantity > 0 && p.StockQuantity <= maxStock)
                .OrderBy(p => p.StockQuantity)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<(string Type, int Count, string? SampleImageUrl)>> GetCategoriesAsync()
        {
            var cacheKey = "ProductCategories";
            if (_cache.TryGetValue(cacheKey, out IEnumerable<(string, int, string?)> cached))
            {
                return cached;
            }

            // Counts per type — a plain aggregation EF translates cleanly.
            var counts = await _context.Products
                .AsNoTracking()
                .Where(p => !p.IsDeleted)
                .GroupBy(p => p.Type)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .OrderBy(x => x.Type)
                .ToListAsync();

            // One sample image per type, fetched separately to avoid a nested
            // navigation projection inside the GroupBy (which EF can't translate).
            var samples = await _context.Products
                .AsNoTracking()
                .Where(p => !p.IsDeleted && p.ProductImages.Any())
                .OrderByDescending(p => p.CreatedOn)
                .Select(p => new { p.Type, Url = p.ProductImages.First().Url })
                .ToListAsync();

            var sampleByType = samples
                .GroupBy(s => s.Type)
                .ToDictionary(g => g.Key, g => g.First().Url);

            var result = counts
                .Select(x => (
                    x.Type,
                    x.Count,
                    (string?)(sampleByType.TryGetValue(x.Type, out var url) ? url : null)))
                .ToList();

            _cache.Set(cacheKey, result, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                Size = 1
            });

            return result;
        }

        public async Task<IEnumerable<Product>> GetAllPagedAsAdminAsync(int pageNumber, int pageSize, string? type = null)
        {
            var hasType = !string.IsNullOrWhiteSpace(type);
            var cacheable = !hasType && pageNumber == 1 && pageSize == DefaultPageSize;
            var cacheKey = $"ProductsPageAdmin-{pageNumber}-{pageSize}";

            if (cacheable && _cache.TryGetValue(cacheKey, out IEnumerable<Product> cached))
            {
                return cached;
            }

            var query = _context.Products
                                .Include(p => p.ProductImages)
                                .AsQueryable();

            if (hasType)
            {
                query = query.Where(p => p.Type == type);
            }

            var products = await query
                                     .OrderByDescending(p => p.CreatedOn)
                                     .Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToListAsync();

            if (cacheable)
            {
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    Size = 1
                };

                _cache.Set(cacheKey, products, cacheOptions);
            }

            return products;
        }
    }
}
