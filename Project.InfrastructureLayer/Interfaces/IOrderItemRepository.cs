using Project.InfrastructureLayer.Entities;

namespace Project.InfrastructureLayer.Interfaces
{
    public interface IOrderItemRepository
    {
        public Task<IEnumerable<OrderItem>> GetByIdAsync(Guid id); 
        public Task AddAsync(OrderItem orderItem);
    }
}
