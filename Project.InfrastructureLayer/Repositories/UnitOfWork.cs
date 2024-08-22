using Project.InfrastructureLayer.Entities;
using Project.InfrastructureLayer.Interfaces;

namespace Project.InfrastructureLayer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private ICustomerRepository _customerRepository;
        private IProductRepository _productRepository;
        private IOrderRepository _orderRepository;
        private IOrderItemRepository _orderItemRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public ICustomerRepository Customers => _customerRepository ??= new CustomerRepository(_context);
        public IProductRepository Products => _productRepository ??= new ProductRepository(_context);
        public IOrderRepository Orders => _orderRepository ??= new OrderRepository(_context);
        public IOrderItemRepository OrderItems => _orderItemRepository ??= new OrderItemRepository(_context);



        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
