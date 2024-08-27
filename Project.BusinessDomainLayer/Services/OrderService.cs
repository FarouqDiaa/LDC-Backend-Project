using AutoMapper;
using Project.BusinessDomainLayer.DTOs;
using Project.InfrastructureLayer.Entities;
using Project.InfrastructureLayer.Interfaces;
using Project.BusinessDomainLayer.Interfaces;
using Project.BusinessDomainLayer.VMs;
using OpenQA.Selenium;

namespace Project.BusinessDomainLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper , IOrderRepository orderReposiotry, ICustomerRepository customerRepository, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderRepository = orderReposiotry;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        public async Task<OrderDTO> GetOrderByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return null;


            return _mapper.Map<OrderDTO>(order);
        }

        public async Task CreateOrderAsync(NewOrderVM newOrder)
        {
            var order = _mapper.Map<Order>(newOrder);
            
            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null || product.StockQuantity < item.Quantity || item.Quantity == 0)
                {
                    throw new InvalidOperationException($"Product {product.Name} is out of stock");
                }
                product.StockQuantity -= item.Quantity;
                item.OrderId = order.OrderId;
                order.Amount += item.Cost * item.Quantity;
            }
            order.TotalAmount = order.Amount + order.Tax;

            await _orderRepository.AddAsync(order);
            await _unitOfWork.CompleteAsync();
        }


        public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync(int pageNumber, Guid customerId)
        {
            bool exists = await _customerRepository.CustomerExistsWithIdAsync(customerId);
            if (!exists) {
                throw new NotFoundException("User not found");
            }
            int pageCount = 25;
            var orders = await _orderRepository.GetAllPagedAsync(pageNumber, pageCount, customerId);
            if (orders == null) {
                throw new NotFoundException("User has no orders");
            }
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }
        public async Task DeleteOrderAsync(Guid id, Guid customerId)
        {
            bool exists = await _customerRepository.CustomerExistsWithIdAsync(customerId);
            if (!exists)
            {
                throw new NotFoundException("User not found");
            }
            Order order = await _orderRepository.GetByIdAsync(id) ?? throw new NotFoundException("Order not found");
            await _orderRepository.RemoveByIdAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}
