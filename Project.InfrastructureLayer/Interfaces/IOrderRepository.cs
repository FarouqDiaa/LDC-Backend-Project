using Project.InfrastructureLayer.Entities;

namespace Project.InfrastructureLayer.Interfaces
{
    public interface IOrderRepository
    {
        public Task<Order> GetByIdAsync(Guid id);
        public Task AddAsync(Order order);
    }
}
