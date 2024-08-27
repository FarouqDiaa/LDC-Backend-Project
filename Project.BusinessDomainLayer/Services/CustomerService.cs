using Project.BusinessDomainLayer.DTOs;
using Project.BusinessDomainLayer.Interfaces;
using Project.InfrastructureLayer.Interfaces;
using Project.InfrastructureLayer.Entities;
using AutoMapper;
using Project.BusinessDomainLayer.VMs;
using OpenQA.Selenium;

namespace Project.BusinessDomainLayer.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IEncryption _encryption;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper, IEncryption encryption, ICustomerRepository customerRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _encryption = encryption;
            _customerRepository = customerRepository;
        }

        public async Task<CustomerDTO> GetCustomerByIdAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) throw new NotFoundException("Customer not found");


            return _mapper.Map<CustomerDTO>(customer);
        }

        public async Task CreateCustomerAsync(NewCustomerVM newCustomer)
        {
            var existingCustomer = await _customerRepository.GetByEmailAsync(newCustomer.Email);
            if (existingCustomer != null) {
            throw new InvalidOperationException("Email already used");
            }
            var customer = GeneratePassword(_mapper.Map<Customer>(newCustomer), newCustomer.Password);
            await _customerRepository.AddAsync(customer);
            await _unitOfWork.CompleteAsync();
        }

        public  Customer GeneratePassword(Customer customer,string password)
        {
            var passwordSalt = _encryption.GenerateSaltedPassword();
            var passwordHash = _encryption.GenerateEncryptedPassword(passwordSalt, password);
            customer.PasswordSalt = passwordSalt;
            customer.PasswordHash = passwordHash;
            return customer;

        }

        public async Task<CustomerDTO> AuthenticateAsync(string email, string password)
        {
            Customer customer = await _customerRepository.GetByEmailAsync(email) ?? throw new ArgumentNullException(nameof(CustomerDTO), "Email not registered");
            bool isValidPassword = _encryption.ValidateEncryptedData(password, customer.PasswordHash, customer.PasswordSalt);

            if (!isValidPassword)throw new InvalidOperationException("Username or password is incorrect");

            var customerDto = _mapper.Map<CustomerDTO>(customer);
            return customerDto;
        }

        public async Task<CustomerDTO> GetCustomerByEmailAsync(string email)
        {
            var customer = await _customerRepository.GetByEmailAsync(email);

            var customerDto = _mapper.Map<CustomerDTO>(customer);
            return customerDto;
        }

        public async Task<bool> IsTheUserAdmin(Guid id) {
            return await _customerRepository.IsAdmin(id);

        }
    }
}
