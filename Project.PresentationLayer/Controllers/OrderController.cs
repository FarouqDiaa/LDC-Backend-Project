using AutoMapper;
using Project.BusinessDomainLayer.Abstractions;
using Project.BusinessDomainLayer.DTOs;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using Project.BusinessDomainLayer.VMs;
using Project.BusinessDomainLayer.Responses;

namespace Project.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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


        [HttpPost("addorder")]
        public async Task<IActionResult> AddOrder(OrderVM newOrder)
        {
            try
            {
                var newOrderDTO = _mapper.Map<NewOrderDTO>(newOrder);
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
            catch (InvalidOperationException e)
            {
                return Conflict(new { Message = e.Message });
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { Message = e.Message });
            }
            catch (NotFoundException e)
            {
                return NotFound(new { Message = e.Message });
            }
        }

        [HttpGet("getallorders/{customerId}")]
        public async Task<IActionResult> GetAllOrders(Guid customerId, [FromQuery] int pageNumber = 1)
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync(pageNumber, customerId);
                return Ok(orders);
            }
            catch (NotFoundException e)
            {
                return NotFound(new {Message = e.Message });
            }
        }


        [HttpDelete("deleteorder/{id}/{customerId}")]
        public async Task<IActionResult> DeleteOrder(Guid id, Guid customerId)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id, customerId);
                return Ok("Order deleted successfully");

            }
            catch (NotFoundException e)
            {
                return NotFound(new { Message = e.Message });
            }
        }
    }
}


//[HttpPost("addorder")]
//[Authorize]
//public async Task<IActionResult> AddOrder(NewOrderDTO newOrder)
//{
//    await _orderService.CreateOrderAsync(newOrder);
//    return Ok("Order added successfully");
//}
