using Project.BusinessDomainLayer.DTOs;
using Project.BusinessDomainLayer.VMs;
using Project.InfrastructureLayer.Entities;

namespace Project.BusinessDomainLayer.Abstractions
{
    public interface ICustomerService
    {
        public Task<CustomerDTO> GetCustomerByIdAsync(Guid id);
        public Task<CustomerDTO> CreateCustomerAsync(NewCustomerDTO newCustomer);
        public Task<CustomerDTO> AuthenticateAsync(LogInDTO logInDTO);
        public Task<CustomerDTO> GetCustomerByEmailAsync(string email);
        public Customer GeneratePassword(Customer customer, string password);
        public Task<bool> IsTheUserAdmin(Guid id);
    }
}
