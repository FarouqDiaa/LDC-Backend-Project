using Project.BusinessDomainLayer.DTOs;

namespace Project.BusinessDomainLayer.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDTO> GetOrderByIdAsync(Guid id);
        Task CreateOrderAsync(NewOrderDTO newOrderDto);
    }
}
