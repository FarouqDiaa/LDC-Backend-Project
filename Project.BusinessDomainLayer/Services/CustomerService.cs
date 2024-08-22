using Project.BusinessDomainLayer.DTOs;
using Project.BusinessDomainLayer.Interfaces;
using Project.InfrastructureLayer.Interfaces;
using Project.InfrastructureLayer.Entities;
using AutoMapper;
using Project.InfrastructureLayer.Repositories;

namespace Project.BusinessDomainLayer.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IEncryption _encryption;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper, IEncryption encryption)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _encryption = encryption;
        }

        public async Task<CustomerDTO> GetCustomerByIdAsync(Guid id)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customer == null) return null;


            return _mapper.Map<CustomerDTO>(customer);
        }

        public async Task CreateCustomerAsync(CustomerDTO newCustomer)
        {
            var customer = _mapper.Map<Customer>(newCustomer);
            await _unitOfWork.Customers.AddAsync(customer);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<CustomerDTO> AuthenticateAsync(string username, string password)
        {
            var customer = await _unitOfWork.Customers.GetByUsernameAsync(username);

            if (customer == null)
            {
                return null;
            }

            bool isValidPassword = await _encryption.ValidateEncryptedData(password, customer.PasswordHash, customer.PasswordSalt);

            if (!isValidPassword)
            {
                return null;
            }

            var customerDto = _mapper.Map<CustomerDTO>(customer);
            return customerDto;
        }

        public async Task<CustomerDTO> GetCustomerByUsernameAsync(string username)
        {
            var customer = await _unitOfWork.Customers.GetByUsernameAsync(username);

            var customerDto = _mapper.Map<CustomerDTO>(customer);
            return customerDto;
        }
    }
}
