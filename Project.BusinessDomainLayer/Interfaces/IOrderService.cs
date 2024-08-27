using Project.BusinessDomainLayer.DTOs;
using Project.BusinessDomainLayer.VMs;

namespace Project.BusinessDomainLayer.Interfaces
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderDTO>> GetAllOrdersAsync(int pageNumber, Guid customerId);
        public Task<OrderDTO> GetOrderByIdAsync(Guid id);
        public Task CreateOrderAsync(NewOrderVM newOrderDto);
        public Task DeleteOrderAsync(Guid id, Guid customerId);
    }
}
