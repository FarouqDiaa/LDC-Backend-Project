using Project.BusinessDomainLayer.DTOs;
using Project.InfrastructureLayer.Entities;

namespace Project.BusinessDomainLayer.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerDTO> GetCustomerByIdAsync(Guid id);
        Task CreateCustomerAsync(CustomerDTO newCustomer);
        Task<CustomerDTO> AuthenticateAsync(string username, string password);
        Task<CustomerDTO> GetCustomerByUsernameAsync(string username);
    }
}
