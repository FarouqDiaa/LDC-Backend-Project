using Project.BusinessDomainLayer.DTOs;
using Project.BusinessDomainLayer.VMs;
using Project.InfrastructureLayer.Entities;
namespace Project.BusinessDomainLayer.Interfaces
{
    public interface ICustomerService
    {
        public Task<CustomerDTO> GetCustomerByIdAsync(Guid id);
        public Task CreateCustomerAsync(NewCustomerVM newCustomer);
        public Task<CustomerDTO> AuthenticateAsync(string email, string password);
        public Task<CustomerDTO> GetCustomerByEmailAsync(string email);
        public Task<Customer> GeneratePassword(Customer customer, string password);
        public Task<bool> IsTheUserAdmin(Guid id);
    }
}
