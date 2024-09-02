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
                product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

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


        public async Task<IEnumerable<Product>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            var cacheKey = $"ProductsPage-{pageNumber}";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Product> products))
            {
                products = await _context.Products
                                         .AsNoTracking()
                                         .Where(p => p.IsDeleted == false)
                                         .Skip((pageNumber - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToListAsync();

                if (pageNumber == 1)
                {
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60),
                        Size = 1
                    };

                    _cache.Set(cacheKey, products, cacheOptions);
                }
            }

            return products;
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

            _cache.Remove($"ProductsPage-1");
            _cache.Remove($"ProductsPageAdmin-1");


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
            _cache.Remove($"ProductsPage-1");
            _cache.Remove($"ProductsPageAdmin-1");
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
            _cache.Remove($"ProductsPage-1");
            _cache.Remove($"ProductsPageAdmin-1");
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

        public async Task<IEnumerable<Product>> GetAllPagedAsAdminAsync(int pageNumber, int pageSize)
        {
            var cacheKey = $"ProductsPageAdmin-{pageNumber}";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Product> products))
            {
                products = await _context.Products
                                         .Skip((pageNumber - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToListAsync();

                if (pageNumber == 1)
                {
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                        Size = 1
                    };

                    _cache.Set(cacheKey, products, cacheOptions);
                }
            }

            return products;
        }
    }
}
