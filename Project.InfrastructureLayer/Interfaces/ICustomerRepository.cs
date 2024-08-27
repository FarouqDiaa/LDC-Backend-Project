using Project.InfrastructureLayer.Entities;

namespace Project.InfrastructureLayer.Interfaces
{
    public interface ICustomerRepository
    {
        public Task<Customer> GetByIdAsync(Guid id);
        public Task AddAsync(Customer customer);
        public Task<Customer> GetByEmailAsync(string email);

        public Task<bool> CustomerExistsAsync(string email);

        public Task<bool> IsAdmin(Guid id);
    }
}
