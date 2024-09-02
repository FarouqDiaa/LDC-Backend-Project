using AutoMapper;
using Project.BusinessDomainLayer.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Project.BusinessDomainLayer.VMs;
using System.ComponentModel.DataAnnotations;
using Project.BusinessDomainLayer.DTOs;
using Project.BusinessDomainLayer.Responses;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Project.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        //private readonly IJWTService _jwtService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerService customerService, IMapper mapper, ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            _mapper = mapper;
            //_jwtService = jwtService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody][Required] CustomerVM customerVM)
        {
            try
            {
                var newCustomerDTO = _mapper.Map<NewCustomerDTO>(customerVM);
                var customer = await _customerService.CreateCustomerAsync(newCustomerDTO);
                var customerResponse = _mapper.Map<CustomerResVM>(customer);
                var successResponse = new SuccessResponse<CustomerResVM>
                {
                    StatusCode = 200,
                    Message = "Customer Added Successfully",
                    Data = customerResponse
                };
                return Ok(successResponse);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update exception caught in controller");

                var errorResponse = new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Can’t Add Customer"
                };
                return BadRequest(errorResponse);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL exception caught in controller");

                var errorResponse = new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Can’t Add Customer"
                };
                return BadRequest(errorResponse);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody][Required] LoginVM loginVM)
        {
            var loginDTO = _mapper.Map<LoginDTO>(loginVM);
            var customer = await _customerService.AuthenticateAsync(loginDTO);
            var customerResponse = _mapper.Map<CustomerResVM>(customer);
            var successResponse = new SuccessResponse<CustomerResVM>
            {
                StatusCode = 200,
                Message = "Login Successfully",
                Data = customerResponse
            };
            return Ok(successResponse);
        }
    }
}