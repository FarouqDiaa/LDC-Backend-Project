using Project.InfrastructureLayer.Entities;

namespace Project.InfrastructureLayer.Abstractions
{
    public interface IOrderRepository
    {
        public Task<Order> GetByIdAsync(Guid id);
        public Task AddAsync(Order order);
        public Task<IEnumerable<Order>> GetAllPagedAsync(int pageNumber, int pageCount, Guid customerId);
        /// <summary>Every customer's orders — admin only.</summary>
        public Task<IEnumerable<Order>> GetAllPagedAsAdminAsync(int pageNumber, int pageCount);
        public Task<int> GetOrdersCountAsync(Guid? customerId = null);
        public Task RemoveByIdAsync(Guid id);
    }
}
