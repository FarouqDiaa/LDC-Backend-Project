using Project.InfrastructureLayer.Entities;
using Project.InfrastructureLayer.Interfaces;

namespace Project.InfrastructureLayer.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<OrderItem> GetByIdAsync(Guid id)
        {
            return await _context.Set<OrderItem>().FindAsync(id);
        }

        public async Task AddAsync(OrderItem orderItem)
        {
            await _context.Set<OrderItem>().AddAsync(orderItem);
            await _context.SaveChangesAsync();
        }
    }
}
