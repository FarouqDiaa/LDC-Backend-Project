using LDCBackendProject.Core.Interfaces;
using LDCBackendProject.EF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDCBackendProject.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IUserRepository _userRepository;
        private IProductRepository _productRepository;
        private IOrderRepository _orderRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public IUserRepository Users
        => _userRepository ??= new UserRepository(_context);

        public IProductRepository Products => throw new NotImplementedException();

        public IOrderRepository Orders => throw new NotImplementedException();


        //public IProductRepository Products
        //=> _productRepository ??= new ProductRepository(_context);
        //public IOrderRepository Orders
        //=> _orderRepository ??= new OrderRepository(_context);

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
