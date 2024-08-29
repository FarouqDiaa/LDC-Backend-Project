using AutoMapper;
using Project.BusinessDomainLayer.Interfaces;
using Project.BusinessDomainLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Project.BusinessDomainLayer.VMs;
using System.ComponentModel.DataAnnotations;

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
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerService customerService, IMapper mapper, IEncryption encryption, JwtService jwtService, ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            _mapper = mapper;
            _encryption = encryption;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody][Required] LoginVM login)
        {
            try {
                var customer = await _customerService.AuthenticateAsync(login.Email, login.Password);
                return Ok(customer);
            }
            catch (ArgumentNullException e)
            {
                _logger.LogWarning("Email is not registered {email}", login.Email);
                return NotFound(new { Message = e.Message });
            }
            catch (InvalidOperationException e) {
                _logger.LogWarning(e.Message);
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] NewCustomerVM newCustomer)
        {

            try
            {
                await _customerService.CreateCustomerAsync(newCustomer);
                return Ok("Successfully Signed Up");
            }
            catch (InvalidOperationException e)
            {
                _logger.LogWarning(e.Message);
                return BadRequest(new { Message = e.Message });
            }
        }

    }
}



//_logger.LogInformation("SignUp process started for user: {Email}", newCustomerDto.Email);
//var stopwatch = new Stopwatch();

//stopwatch.Start();
//_logger.LogInformation("Checking if user already exists");
//stopwatch.Stop();
//_logger.LogInformation("Time taken to check existing user: {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);

//[HttpPost("signup")]
//public async Task<IActionResult> SignUp([FromBody] NewCustomerDTO newCustomerDto)
//{
//    var existingCustomer = await _customerService.GetCustomerByEmailAsync(newCustomerDto.Name);
//    if (existingCustomer != null)
//    {
//        return Conflict("Email already exists");
//    }

//    var passwordSalt = _encryption.GenerateSaltedPassword();
//    var passwordHash = _encryption.GenerateEncryptedPassword(passwordSalt, newCustomerDto.Password);
//    var customerDto = _mapper.Map<CustomerDTO>(newCustomerDto);
//    customerDto.PasswordSalt = passwordSalt;
//    customerDto.PasswordHash = passwordHash;

//    await _customerService.CreateCustomerAsync(customerDto);

//    return StatusCode(201);
//}