using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Project.InfrastructureLayer.Entities;
using Project.InfrastructureLayer.Interfaces;


namespace Project.InfrastructureLayer.Repositories
{
    public class ProductRepository:IProductRepository
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
            return await _context.Set<Product>().SingleOrDefaultAsync(p => p.ProductId == id);
        }


        public async Task<Product> GetByNameAsync(string name)
        {
            var cacheKey = $"Product-{name}";
            Product product;
            if (!_cache.TryGetValue(cacheKey, out product))
            {
                product = await _context.Products.AsNoTracking()
                                                   .Where(p => p.Name == name)
                                                   .SingleOrDefaultAsync();

                if (product != null)
                {
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    };

                    _cache.Set(cacheKey, product, cacheOptions);
                }
            }

            return product;
        }

        public async Task AddAsync(Product product)
        {
            await _context.Set<Product>().AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Set<Product>().ToListAsync();

        }

        public async Task<IEnumerable<Product>> GetAllPagedAsync(int pageNumber, int pageSize, bool isAdmin)
        {
            if (isAdmin)
            {
                return await _context.Set<Product>()
                                     .Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToListAsync();
            }
            else 
            {

                return await _context.Set<Product>()
                                     .Where(p => p.IsDeleted == false)
                                     .Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToListAsync();
            }
        }
        public void Update(Product product)
        {
            product.UpdatedOn = DateTime.UtcNow;
            _context.Set<Product>().Update(product);
        }


        public async Task RemoveByIdAsync(Guid id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                product.IsDeleted = true;
            }
        }

        public async Task<bool> ProductExistsAsync(Guid id)
        {
            var cacheKey = $"ProductExists-{id}";
            if (!_cache.TryGetValue(cacheKey, out bool exists))
            {
                exists = await _context.Products
                                       .AsNoTracking()
                                       .AnyAsync(p => p.ProductId == id);

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };
                _cache.Set(cacheKey, exists, cacheOptions);

                await CacheRandomProductsAsync(10);
            }

            return exists;
        }
        public async Task CacheRandomProductsAsync(int numberOfProductsToCache)
        {
            var randomProducts = await _context.Products
                                                .AsNoTracking()
                                                .OrderBy(p => new Guid())
                                                .Take(numberOfProductsToCache)
                                                .Select(p => p.ProductId)
                                                .ToListAsync();

            foreach (var id in randomProducts)
            {
                var exists = await _context.Products
                                           .AsNoTracking()
                                           .AnyAsync(p => p.ProductId == id);

                var cacheKey = $"ProductExists-{id}";
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };

                _cache.Set(cacheKey, exists, cacheOptions);
            }
        }
    }
}
