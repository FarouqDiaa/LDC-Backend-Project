using AutoMapper;
using Project.BusinessDomainLayer.DTOs;
using Project.InfrastructureLayer.Entities;
using Project.InfrastructureLayer.Interfaces;
using Project.BusinessDomainLayer.Interfaces;
using Project.BusinessDomainLayer.VMs;
using OpenQA.Selenium;
using Project.InfrastructureLayer.Repositories;

namespace Project.BusinessDomainLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper , IOrderRepository orderReposiotry, ICustomerRepository customerRepository, IProductRepository productRepository, IOrderItemRepository orderItemRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderRepository = orderReposiotry;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _orderItemRepository = orderItemRepository;
        }
        public async Task<OrderDTO> GetOrderByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return null;

            var orderItems = await _orderItemRepository.GetByIdAsync(id);

            var orderItemDTOs = _mapper.Map<ICollection<OrderItemDTO>>(orderItems);

            var orderDTO = _mapper.Map<OrderDTO>(order);
            orderDTO.OrderItems = orderItemDTOs;

            return orderDTO;
        }

        public async Task CreateOrderAsync(NewOrderVM newOrder, Guid customerId)
        {
            bool exists = await _customerRepository.CustomerExistsWithIdAsync(customerId);
            if (!exists)
            {
                throw new KeyNotFoundException("Customer not found");
            }

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var order = _mapper.Map<Order>(newOrder);
                    order.OrderItems = null;
                    order.CustomerId = customerId;
                    order.Amount = 0;

                    await _orderRepository.AddAsync(order);
                    await _unitOfWork.CompleteAsync();

                    foreach (var item in newOrder.OrderItems)
                    {
                        var product = await _productRepository.GetByIdAsync(item.ProductId);

                        if (product == null)
                        {
                            throw new NotFoundException("One of the products is not found");
                        }

                        if (product.IsDeleted)
                        {
                            throw new NotFoundException($"{product.Name} is deleted");
                        }

                        if (product.StockQuantity < item.Quantity)
                        {
                            throw new InvalidOperationException($"{product.Name} doesn't have enough quantities");
                        }

                        product.StockQuantity -= item.Quantity;

                        var orderItem = new OrderItem
                        {
                            OrderId = order.OrderId,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            Cost = product.Cost * item.Quantity
                        };

                        await _orderItemRepository.AddAsync(orderItem);

                        order.Amount += orderItem.Cost;
                    }

                    order.TotalAmount = order.Amount + order.Tax;

                    await _unitOfWork.CompleteAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }



        public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync(int pageNumber, Guid customerId)
        {
            bool exists = await _customerRepository.CustomerExistsWithIdAsync(customerId);
            if (!exists) {
                throw new NotFoundException("User not found");
            }
            int pageCount = 25;
            var orders = await _orderRepository.GetAllPagedAsync(pageNumber, pageCount, customerId);
            return orders == null ? throw new NotFoundException("User has no orders") : _mapper.Map<IEnumerable<OrderDTO>>(orders);
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


//public async Task CreateOrderAsync(NewOrderVM newOrder, Guid customerId)
//{
//    bool exists = await _customerRepository.CustomerExistsWithIdAsync(customerId);
//    if (!exists)
//    {
//        throw new KeyNotFoundException("Customer not found");
//    }
//    var order = _mapper.Map<Order>(newOrder);

//    foreach (var item in order.OrderItems)
//    {
//        var product = await _productRepository.GetByIdAsync(item.ProductId);
//        if (product.IsDeleted)
//        {
//            throw new NotFoundException("One of the products is not found");
//        }
//        if (product == null || product.StockQuantity < item.Quantity)
//        {
//            throw new InvalidOperationException($"Product {product?.Name} doesn't have enough quantities");
//        }

//        product.StockQuantity -= item.Quantity;
//        item.Cost = product.Cost * item.Quantity;
//        item.OrderId = order.OrderId;
//        order.Amount += item.Cost;

//        await _orderItemRepository.AddAsync(item);
//    }

//    order.TotalAmount = order.Amount + order.Tax;

//    await _orderRepository.AddAsync(order);
//    await _unitOfWork.CompleteAsync();
//}