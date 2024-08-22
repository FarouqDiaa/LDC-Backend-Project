using Project.InfrastructureLayer.Entities;
using Project.InfrastructureLayer.Interfaces;

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
            return await _context.Set<Order>().FindAsync(id);
        }

        public async Task AddAsync(Order order)
        {
            await _context.Set<Order>().AddAsync(order);
            await _context.SaveChangesAsync();
        }
    }
}
