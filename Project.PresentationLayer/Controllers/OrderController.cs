using AutoMapper;
using Project.BusinessDomainLayer.Interfaces;
using Project.BusinessDomainLayer.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Project.BusinessDomainLayer.Services;

namespace Project.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, IMapper mapper, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("getallorders/{customerId}")]
        public async Task<IActionResult> GetAllOrders(Guid customerId, [FromQuery] int pageNumber = 1)
        {
            var orders = await _orderService.GetAllOrdersAsync(pageNumber, customerId);
            if (orders == null) { 
            return NotFound("This cutomer has no orders");
            }
            return Ok(orders);
        }

        [HttpPost("addorder")]
        public async Task<IActionResult> AddOrder(NewOrderDTO newOrder)
        {
            await _orderService.CreateOrderAsync(newOrder);
            return Ok("Order added successfully");
        }

        [HttpDelete("deleteorder/{id}")]
        //[Authorize]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {

            //var isAdminClaim = User.FindFirst("IsAdmin")?.Value;
            string isAdminClaim = "1";
            if (isAdminClaim != null && bool.TryParse(isAdminClaim, out bool isAdmin) && isAdmin)
            {
                var existingOrder = await _orderService.GetOrderByIdAsync(id);
                if (existingOrder == null)
                {
                    return NotFound("Order not found");
                }

                await _orderService.DeleteOrderAsync(id);

                return Ok("Order deleted successfully");
            }
            return Conflict("Not and admin");
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
