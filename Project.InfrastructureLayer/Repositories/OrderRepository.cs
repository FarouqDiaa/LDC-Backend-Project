using Microsoft.EntityFrameworkCore;
using Project.InfrastructureLayer.Entities;
using Project.InfrastructureLayer.Interfaces;
using System.Linq;

namespace Project.InfrastructureLayer.Repositories
{
    public class OrderRepository:IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetByIdAsync(Guid id)
        {
            return await _context.Set<Order>().SingleOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task AddAsync(Order order)
        {
            await _context.Set<Order>().AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Set<Order>().ToListAsync();

        }

        public async Task<IEnumerable<Order>> GetAllPagedAsync(int pageNumber, int pageCount, Guid customerId)
        {
            return await _context.Set<Order>()
                                 .Where(o => o.CustomerId == customerId)
                                 .Skip((pageNumber - 1) * pageCount)
                                 .Take(pageCount)
                                 .ToListAsync();
        }

        public async Task RemoveByIdAsync(Guid id)
        {
            var order = await GetByIdAsync(id);
            if (order != null)
            {
                order.IsDeleted = true;
            }
        }
    }
}
