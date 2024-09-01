using Project.BusinessDomainLayer.DTOs;

namespace Project.BusinessDomainLayer.Abstractions
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderDTO>> GetAllOrdersAsync(int pageNumber, Guid customerId);
        public Task<OrderDTO> GetOrderByIdAsync(Guid id);
        public Task CreateOrderAsync(NewOrderDTO newOrderDto);
        public Task DeleteOrderAsync(Guid id, Guid customerId);
    }
}
