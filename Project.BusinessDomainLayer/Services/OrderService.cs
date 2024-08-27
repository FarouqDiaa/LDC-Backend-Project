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
            order.CustomerId = newOrderDto.CustomerId;

            foreach (var item in newOrderDto.OrderItems)
            {
                var orderItem = _mapper.Map<OrderItem>(item);
                order.OrderItems.Add(orderItem);
            }

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CompleteAsync();
        }


        public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync(int pageNumber, Guid customerId)
        {
            int pageCount = 25;
            var orders = await _unitOfWork.Orders.GetAllPagedAsync(pageNumber, pageCount, customerId);
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }
        public async Task DeleteOrderAsync(Guid id)
        {
            Order order = await _unitOfWork.Orders.GetByIdAsync(id) ?? throw new KeyNotFoundException("Order not found");
            await _unitOfWork.Orders.RemoveByIdAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}
