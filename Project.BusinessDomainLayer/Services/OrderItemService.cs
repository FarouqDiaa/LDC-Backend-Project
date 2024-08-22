using AutoMapper;
using Project.BusinessDomainLayer.DTOs;
using Project.InfrastructureLayer.Entities;
using Project.BusinessDomainLayer.Interfaces;
using Project.InfrastructureLayer.Interfaces;

namespace Project.BusinessDomainLayer.Services
{
    public class OrderItemService : IOrderItemService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrderItemDTO> GetOrderItemByIdAsync(Guid id)
        {
            var orderItem = await _unitOfWork.OrderItems.GetByIdAsync(id);
            if (orderItem == null) return null;


            return _mapper.Map<OrderItemDTO>(orderItem);
        }

        public async Task CreateOrderItemAsync(NewOrderItemDTO newOrderItemDto)
        {
            var orderItem = _mapper.Map<OrderItem>(newOrderItemDto);
            await _unitOfWork.OrderItems.AddAsync(orderItem);
            await _unitOfWork.CompleteAsync();
        }

    }
}
