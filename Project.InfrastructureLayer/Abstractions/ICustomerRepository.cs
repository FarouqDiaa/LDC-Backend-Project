using Project.InfrastructureLayer.Entities;

namespace Project.InfrastructureLayer.Abstractions
{
    public interface ICustomerRepository
    {
        public Task<Customer> GetByIdAsync(Guid id);
        public Task AddAsync(Customer customer);
        public Task<Customer> GetByEmailAsync(string email);

        public Task<bool> IsCustomerExistsAsync(string email);
        public Task<bool> IsCustomerExistsByIdAsync(Guid id);

        public Task<bool> IsAdmin(Guid id);

        /// <summary>All customers with their order stats — admin only.</summary>
        public Task<IEnumerable<Customer>> GetAllPagedAsync(int pageNumber, int pageSize);
        public Task<int> GetCustomersCountAsync();
    }
}
