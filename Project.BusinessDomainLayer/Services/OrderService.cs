using AutoMapper;
using Project.BusinessDomainLayer.DTOs;
using Project.InfrastructureLayer.Entities;
using Project.InfrastructureLayer.Abstractions;
using Project.BusinessDomainLayer.Abstractions;
using Project.BusinessDomainLayer.Exceptions.CustomerExceptions;
using Project.BusinessDomainLayer.Exceptions.ProductExceptions;
using Project.BusinessDomainLayer.Exceptions.OrderExceptions;

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

        private static readonly decimal TaxRate = 0.14m;
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper , IOrderRepository orderReposiotry, ICustomerRepository customerRepository, IProductRepository productRepository, IOrderItemRepository orderItemRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderRepository = orderReposiotry;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _orderItemRepository = orderItemRepository;
        }
        public async Task<OrderDTO> CreateOrderAsync(NewOrderDTO newOrder)
        {

            bool exists = await _customerRepository.IsCustomerExistsByIdAsync(newOrder.CustomerId);
            if (!exists)
            {
                throw new FailedCustomerActionException("Customer doesn't exist");
            }

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var order = _mapper.Map<Order>(newOrder);
                    order.OrderItems = new List<OrderItem>();
                    order.CustomerId = newOrder.CustomerId;
                    await _orderRepository.AddAsync(order);
                    await _unitOfWork.CompleteAsync();

                    foreach (var item in newOrder.OrderItems)
                    {
                        var product = await _productRepository.GetByIdAsync(item.ProductId) ?? throw new ProductNotFoundException("One of the products is not found");

                        if (product.IsDeleted)
                        {
                            throw new ProductNotFoundException($"{product.Name} is deleted");
                        }

                        if (product.StockQuantity < item.Quantity)
                        {
                            throw new ProductQuantityException($"{product.Name} doesn't have enough quantities");
                        }
                        var existingOrderItem = order.OrderItems.FirstOrDefault(oi => oi.ProductId == item.ProductId);

                        if (existingOrderItem != null)
                        {
                            existingOrderItem.Quantity += item.Quantity;
                            existingOrderItem.Cost += product.Cost * item.Quantity;
                        }
                        else
                        {
                            product.StockQuantity -= item.Quantity;

                            var orderItem = _mapper.Map<OrderItem>(item);
                            orderItem.OrderId = order.Id;
                            orderItem.Cost = product.Cost * item.Quantity;

                            order.OrderItems.Add(orderItem);
                        }

                        order.Amount += product.Cost * item.Quantity;
                    }

                    order.TotalAmount = Math.Round(order.Amount + ((float)TaxRate * order.Amount), 2); ;
                    order.Tax = (float)TaxRate;
                    await _unitOfWork.CompleteAsync();
                    await transaction.CommitAsync();
                    return _mapper.Map<OrderDTO>(order);
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
            bool exists = await _customerRepository.IsCustomerExistsByIdAsync(customerId);
            if (!exists) {
                throw new InvalidCustomerIdException("InValid Customer Id");
            }
            int pageCount = 25;
            var orders = await _orderRepository.GetAllPagedAsync(pageNumber, pageCount, customerId);
            return orders == null ? throw new OrderNotFoundException("User has no orders") : _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            Order order = await _orderRepository.GetByIdAsync(id) ?? throw new OrderNotFoundException("InValid Order Id");
            await _orderRepository.RemoveByIdAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}
