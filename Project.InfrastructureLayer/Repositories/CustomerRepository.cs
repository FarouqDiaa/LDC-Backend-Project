using Microsoft.EntityFrameworkCore;
using Project.InfrastructureLayer.Entities;
using Project.InfrastructureLayer.Interfaces;

namespace Project.InfrastructureLayer.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> GetByIdAsync(Guid id)
        {
            return await _context.Set<Customer>().FindAsync(id);
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.Set<Customer>().AddAsync(customer);
        }

        public async Task<Customer> GetByUsernameAsync(string username)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Name == username);
        }

    }
}
