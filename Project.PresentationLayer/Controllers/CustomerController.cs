using AutoMapper;
using Project.BusinessDomainLayer.Interfaces;
using Project.BusinessDomainLayer.DTOs;
using Project.BusinessDomainLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Project.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        private readonly IEncryption _encryption;
        private readonly JwtService _jwtService;

        public CustomerController(ICustomerService customerService, IMapper mapper, IEncryption encryption, JwtService jwtService)
        {
            _customerService = customerService;
            _mapper = mapper;
            _encryption = encryption;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerDto = await _customerService.AuthenticateAsync(loginDto.Username, loginDto.Password);
            if (customerDto == null) return Unauthorized();

            var token = _jwtService.GenerateToken(customerDto.CustomerId, customerDto.Name, customerDto.IsAdmin);

            return Ok(new { Token = token });
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] NewCustomerDTO newCustomerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCustomer = await _customerService.GetCustomerByUsernameAsync(newCustomerDto.Name);
            if (existingCustomer != null)
            {
                return Conflict("Username already exists");
            }

            var passwordSalt = await _encryption.GenerateSaltedPassword();
            var passwordHash = await _encryption.GenerateEncryptedPassword(passwordSalt, newCustomerDto.Password);

            var customerDto = new CustomerDTO
            {
                Name = newCustomerDto.Name,
                Address = newCustomerDto.Address,
                Phone = newCustomerDto.Phone,
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash
            };

            await _customerService.CreateCustomerAsync(customerDto);

            return StatusCode(201);
        }
    }
}
