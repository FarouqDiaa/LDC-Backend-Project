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
            var productCacheKey = $"Product-{name}";
            bool exists = false, productExists = false;
            Product product = null;
            if (!_cache.TryGetValue(cacheKey, out exists) && !_cache.TryGetValue(cacheKey, out product))
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
            if (product != null)
            {
                productExists = true;
            }
            return (exists || productExists);
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

        public void Update(Product product)
        {
            product.UpdatedOn = DateTime.UtcNow;
            _context.Products.Update(product);
            _cache.Remove($"ProductsPage-1");
        }

        public async Task AddAsync(Product product)
        {
            product.UpdatedOn = DateTime.UtcNow;
            product.CreatedOn = DateTime.UtcNow;
            await _context.Products.AddAsync(product);
            _cache.Remove($"ProductsPage-1");
        }

        public async Task RemoveByIdAsync(Guid id)
        {
            var product = await _context.Products
                                        .FirstOrDefaultAsync(p => p.Id == id);

            product.IsDeleted = true;
            product.UpdatedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            _cache.Remove($"ProductsPage-1");
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
