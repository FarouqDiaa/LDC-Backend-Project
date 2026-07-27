using Microsoft.EntityFrameworkCore;
using Project.InfrastructureLayer.Entities;
using Project.InfrastructureLayer.Abstractions;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace Project.InfrastructureLayer.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        public OrderRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<Order> GetByIdAsync(Guid id)
        {
            var cacheKey = $"Order-{id}";

            if (!_cache.TryGetValue(cacheKey, out Order order))
            {
                order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);

                if (order != null)
                {
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                        Size = 1
                    };
                    _cache.Set(cacheKey, order, cacheOptions);
                }
            }

            return order;
        }

        private const int DefaultPageSize = 25;

        public async Task AddAsync(Order order)
        {
            order.CreatedOn = DateTime.UtcNow;
            order.UpdatedOn = DateTime.UtcNow;
            _cache.Remove($"OrdersPage-1-{DefaultPageSize}-{order.CustomerId}");
            await _context.Orders.AddAsync(order);
        }


        public async Task<IEnumerable<Order>> GetAllPagedAsAdminAsync(int pageNumber, int pageCount)
        {
            return await _context.Orders
                         .AsNoTracking()
                         .Where(o => !o.IsDeleted)
                         .Include(o => o.OrderItems)
                         .Include(o => o.Customer)
                         .OrderByDescending(o => o.CreatedOn)
                         .Skip((pageNumber - 1) * pageCount)
                         .Take(pageCount)
                         .ToListAsync();
        }

        public async Task<int> GetOrdersCountAsync(Guid? customerId = null)
        {
            var query = _context.Orders.AsNoTracking().Where(o => !o.IsDeleted);

            if (customerId is not null)
            {
                query = query.Where(o => o.CustomerId == customerId.Value);
            }

            return await query.CountAsync();
        }

        public async Task<IEnumerable<Order>> GetAllPagedAsync(int pageNumber, int pageCount, Guid customerId)
        {
            var cacheable = pageNumber == 1 && pageCount == DefaultPageSize;
            var cacheKey = $"OrdersPage-{pageNumber}-{pageCount}-{customerId}";

            if (cacheable && _cache.TryGetValue(cacheKey, out IEnumerable<Order> cached))
            {
                return cached;
            }

            var orders = await _context.Orders
                             .AsNoTracking()
                             .Where(o => o.CustomerId == customerId && !o.IsDeleted)
                             .Include(o => o.OrderItems)
                             .Include(o => o.Customer)
                             .OrderByDescending(o => o.CreatedOn)
                             .Skip((pageNumber - 1) * pageCount)
                             .Take(pageCount)
                             .ToListAsync();

            if (cacheable)
            {
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2),
                    Size = 1
                };

                _cache.Set(cacheKey, orders, cacheOptions);
            }

            return orders;
        }

        public async Task RemoveByIdAsync(Guid id)
        {
            var order = await GetByIdAsync(id);
            order.UpdatedOn = DateTime.UtcNow;
            order.IsDeleted = true;
            _cache.Remove($"OrdersPage-1-{DefaultPageSize}-{order.CustomerId}");
            _cache.Remove($"Order-{id}");
        }
    }
}
