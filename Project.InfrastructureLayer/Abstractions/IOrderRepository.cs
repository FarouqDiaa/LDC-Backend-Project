using Project.InfrastructureLayer.Entities;

namespace Project.InfrastructureLayer.Abstractions
{
    public interface IOrderRepository
    {
        public Task<Order> GetByIdAsync(Guid id);
        public Task AddAsync(Order order);
        public Task<IEnumerable<Order>> GetAllPagedAsync(int pageNumber, int pageCount, Guid customerId);
        public Task RemoveByIdAsync(Guid id);
    }
}
