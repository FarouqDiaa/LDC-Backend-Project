using Project.BusinessDomainLayer.DTOs;
using Project.BusinessDomainLayer.Abstractions;
using Project.InfrastructureLayer.Abstractions;
using Project.InfrastructureLayer.Entities;
using AutoMapper;
using Project.BusinessDomainLayer.VMs;
using OpenQA.Selenium;
using Project.InfrastructureLayer.Migrations;
using Project.BusinessDomainLayer.Exceptions.CustomerExceptions;

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

        public async Task<CustomerDTO> CreateCustomerAsync(NewCustomerDTO newCustomer)
        {
            var existingCustomer = await _customerRepository.IsCustomerExistsAsync(newCustomer.Email);
            if (existingCustomer == true)
            {
                throw new UsedEmailException("Email already used");
            }
            var customer = GeneratePassword(_mapper.Map<Customer>(newCustomer), newCustomer.Password);
            await _customerRepository.AddAsync(customer);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<CustomerDTO>(customer);
        }

        private Customer GeneratePassword(Customer customer, string password)
        {
            var passwordSalt = _encryption.GenerateSaltedPassword();
            var passwordHash = _encryption.GenerateEncryptedPassword(passwordSalt, password);
            customer.PasswordSalt = passwordSalt;
            customer.PasswordHash = passwordHash;
            return customer;

        }

        public async Task<CustomerDTO> GetCustomerByIdAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) throw new CustomerNotFoundException("Customer not found");


            return _mapper.Map<CustomerDTO>(customer);
        }



        public async Task<CustomerDTO> AuthenticateAsync(LoginDTO logInDTO)
        {
            string email = logInDTO.Email, password = logInDTO.Password;

            Customer customer = await _customerRepository.GetByEmailAsync(email) ?? throw new EmailNotRegisteredException("Email not registered");
            bool isValidPassword = _encryption.ValidateEncryptedData(password, customer.PasswordHash, customer.PasswordSalt);

            if (!isValidPassword)throw new AuthenticationException("Email or password is incorrect");

            var customerDto = _mapper.Map<CustomerDTO>(customer);
            return customerDto;
        }

        public async Task<CustomerDTO> GetCustomerByEmailAsync(string email)
        {
            var customer = await _customerRepository.GetByEmailAsync(email);
            if (customer == null) throw new CustomerNotFoundException("Customer not found");
            var customerDto = _mapper.Map<CustomerDTO>(customer);
            return customerDto;
        }

        public async Task<bool> IsTheUserAdmin(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) throw new CustomerNotFoundException("Customer not found");

            return await _customerRepository.IsAdmin(id);

        }
    }
}
