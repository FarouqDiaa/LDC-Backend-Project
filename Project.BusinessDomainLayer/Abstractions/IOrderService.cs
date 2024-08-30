using Project.BusinessDomainLayer.DTOs;
using Project.BusinessDomainLayer.VMs;

namespace Project.BusinessDomainLayer.Abstractions
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderDTO>> GetAllOrdersAsync(int pageNumber, Guid customerId);
        public Task<OrderDTO> GetOrderByIdAsync(Guid id);
        public Task CreateOrderAsync(NewOrderDTO newOrderDto, Guid customerId);
        public Task DeleteOrderAsync(Guid id, Guid customerId);
    }
}
