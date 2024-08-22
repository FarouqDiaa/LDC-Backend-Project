using Project.InfrastructureLayer.Entities;

namespace Project.InfrastructureLayer.Interfaces
{
    public interface IOrderItemRepository
    {
        public Task<OrderItem> GetByIdAsync(Guid id); 
        public Task AddAsync(OrderItem orderItem);
    }
}
