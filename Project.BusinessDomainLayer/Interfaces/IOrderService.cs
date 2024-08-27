using Project.BusinessDomainLayer.DTOs;

namespace Project.BusinessDomainLayer.Interfaces
{
    public interface IOrderService
    {
        public Task<OrderDTO> GetOrderByIdAsync(Guid id);
        public Task CreateOrderAsync(NewOrderDTO newOrderDto);
        public Task<IEnumerable<OrderDTO>> GetAllOrdersAsync(int pageNumber, Guid customerId);
        public Task DeleteOrderAsync(Guid id);
    }
}
