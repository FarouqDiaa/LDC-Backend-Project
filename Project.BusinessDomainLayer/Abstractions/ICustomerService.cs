using Project.BusinessDomainLayer.DTOs;
namespace Project.BusinessDomainLayer.Abstractions
{
    public interface ICustomerService
    {
        public Task<CustomerDTO> GetCustomerByIdAsync(Guid id);
        public Task<CustomerDTO> CreateCustomerAsync(NewCustomerDTO newCustomer);
        public Task<CustomerDTO> AuthenticateAsync(LoginDTO loginDTO);
        public Task<CustomerDTO> GetCustomerByEmailAsync(string email);
        public Task<bool> IsTheUserAdmin(Guid id);
    }
}
