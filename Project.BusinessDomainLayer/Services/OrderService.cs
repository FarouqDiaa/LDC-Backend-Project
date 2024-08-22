using AutoMapper;
using Project.BusinessDomainLayer.DTOs;
using Project.InfrastructureLayer.Entities;
using Project.InfrastructureLayer.Interfaces;
using Project.BusinessDomainLayer.Interfaces;

namespace Project.BusinessDomainLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrderDTO> GetOrderByIdAsync(Guid id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null) return null;


            return _mapper.Map<OrderDTO>(order);
        }

        public async Task CreateOrderAsync(NewOrderDTO newOrderDto)
        {
            var order = _mapper.Map<Order>(newOrderDto);
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CompleteAsync();
        }

    }
}
