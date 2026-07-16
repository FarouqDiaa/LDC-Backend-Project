using AutoMapper;
using Project.BusinessDomainLayer.Abstractions;
using Project.BusinessDomainLayer.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Project.BusinessDomainLayer.VMs;
using Project.BusinessDomainLayer.Responses;
using Project.PresentationLayer.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Project.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger, IMapper mapper)
        {
            _orderService = orderService;
            _logger = logger;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<IActionResult> AddOrder(OrderVM newOrder)
        {
            var customerId = User.GetCustomerId();
            if (customerId is null) return Unauthorized();

            try
            {
                var newOrderDTO = _mapper.Map<NewOrderDTO>(newOrder);
                newOrderDTO.CustomerId = customerId.Value;
                var order = await _orderService.CreateOrderAsync(newOrderDTO);

                var orderRes = _mapper.Map<OrderResVM>(order);
                var successResponse = new SuccessResponse<OrderResVM>
                {
                    StatusCode = 200,
                    Message = "Order Added Successfully",
                    Data = orderRes
                };
                return Ok(successResponse);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update exception caught in controller");

                var errorResponse = new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Can’t Add Order"
                };
                return BadRequest(errorResponse);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL exception caught in controller");

                var errorResponse = new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Can’t Add Order"
                };
                return BadRequest(errorResponse);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id); 
                var successResponse = new BaseResponse
                {
                    StatusCode = 200,
                    Message = "Order Deleted Successfully"
                };
                return Ok(successResponse);

            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update exception caught in controller");

                var errorResponse = new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Can’t Delete Order"
                };
                return BadRequest(errorResponse);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL exception caught in controller");

                var errorResponse = new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Can’t Delete Order"
                };
                return BadRequest(errorResponse);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25)
        {
            var customerId = User.GetCustomerId();
            if (customerId is null) return Unauthorized();

            try
            {
                var orders = await _orderService.GetAllOrdersAsync(pageNumber, pageSize, customerId.Value);
                var orderRes = _mapper.Map<IEnumerable<OrderResVM>>(orders);
                var successResponse = new SuccessResponse<IEnumerable<OrderResVM>>
                {
                    StatusCode = 200,
                    Message = "Orders Retrieved Successfully",
                    Data = orderRes
                };
                return Ok(successResponse);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update exception caught in controller");

                var errorResponse = new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Can’t Retrieve Orders"
                };
                return BadRequest(errorResponse);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL exception caught in controller");

                var errorResponse = new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Can’t Retrieve Orders"
                };
                return BadRequest(errorResponse);
            }
        }
    }
}