using Project.BusinessDomainLayer.DTOs;

namespace Project.BusinessDomainLayer.Interfaces
{
    public interface IOrderItemService
    {
        Task<OrderItemDTO> GetOrderItemByIdAsync(Guid id);
        Task CreateOrderItemAsync(NewOrderItemDTO newOrderItemDto);
    }
}
