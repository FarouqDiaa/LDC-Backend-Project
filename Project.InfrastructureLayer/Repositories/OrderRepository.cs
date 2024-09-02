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

        public async Task AddAsync(Order order)
        {
            order.CreatedOn = DateTime.UtcNow;
            order.UpdatedOn = DateTime.UtcNow;
            var cacheKey = $"OrdersPage-1-{order.CustomerId}";
            if (_cache.TryGetValue(cacheKey, out _))
            {
                _cache.Remove(cacheKey);
            }
            await _context.Orders.AddAsync(order);
        }


        public async Task<IEnumerable<Order>> GetAllPagedAsync(int pageNumber, int pageCount, Guid customerId)
        {
            var cacheKey = $"OrderPage-{pageNumber}-{customerId}";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Order> orders))
            {
                orders = await _context.Orders
                                 .Where(o => o.CustomerId == customerId && !o.IsDeleted)
                                 //.Include(o => o.OrderItems)
                                 .Skip((pageNumber - 1) * pageCount)
                                 .Take(pageCount)
                                 .ToListAsync();

                if (pageNumber == 1)
                {
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2),
                        Size = 1
                    };

                    _cache.Set(cacheKey, orders, cacheOptions);
                }
            }

            return orders;
        }

        public async Task RemoveByIdAsync(Guid id)
        {
            var order = await GetByIdAsync(id);
            order.UpdatedOn = DateTime.UtcNow;
            order.IsDeleted = true;
            var cacheKey = $"OrdersPage-1-{order.CustomerId}";
            if (_cache.TryGetValue(cacheKey, out _))
            {
                _cache.Remove(cacheKey);
            }
            _cache.Remove($"Order-{id}");
        }
    }
}
