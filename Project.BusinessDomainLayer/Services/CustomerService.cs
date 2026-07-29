using Project.BusinessDomainLayer.DTOs;
using Project.BusinessDomainLayer.Abstractions;
using Project.InfrastructureLayer.Abstractions;
using Project.InfrastructureLayer.Entities;
using AutoMapper;
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



        public async Task<CustomerDTO> AuthenticateAsync(LoginDTO loginDTO)
        {
            string email = loginDTO.Email, password = loginDTO.Password;

            Customer customer = await _customerRepository.GetByEmailAsync(email) ?? throw new EmailNotRegisteredException("Email not registered");
            bool isValidPassword = _encryption.ValidateEncryptedData(password, customer.PasswordHash, customer.PasswordSalt);

            if (!isValidPassword)throw new AuthenticationException("Email or password is incorrect");

           return _mapper.Map<CustomerDTO>(customer);
        }

        public async Task<CustomerDTO> GetCustomerByEmailAsync(string email)
        {
            var customer = await _customerRepository.GetByEmailAsync(email);
            if (customer == null) throw new CustomerNotFoundException("Customer not found");
            return _mapper.Map<CustomerDTO>(customer);
        }

        public async Task<bool> IsTheUserAdmin(Guid id)
        {
            var customer = await _customerRepository.IsCustomerExistsByIdAsync(id);
            if (customer == false) throw new CustomerNotFoundException("Customer not found");

            return await _customerRepository.IsAdmin(id);

        }

        private const int DefaultPageSize = 25;
        private const int MaxPageSize = 100;

        public async Task<(IEnumerable<CustomerDTO> Items, int TotalCount)> GetAllCustomersAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }
            if (pageSize <= 0 || pageSize > MaxPageSize)
            {
                pageSize = DefaultPageSize;
            }

            var customers = await _customerRepository.GetAllPagedAsync(pageNumber, pageSize);
            var totalCount = await _customerRepository.GetCustomersCountAsync();

            var items = customers.Select(customer =>
            {
                var dto = _mapper.Map<CustomerDTO>(customer);
                var orders = customer.Orders?.Where(o => !o.IsDeleted).ToList() ?? [];
                dto.OrdersCount = orders.Count;
                dto.TotalSpent = orders.Sum(o => o.TotalAmount);
                return dto;
            });

            return (items, totalCount);
        }
    }
}
