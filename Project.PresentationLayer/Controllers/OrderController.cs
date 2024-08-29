using AutoMapper;
using Project.BusinessDomainLayer.Interfaces;
using Project.BusinessDomainLayer.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Project.BusinessDomainLayer.Services;
using OpenQA.Selenium;
using Project.BusinessDomainLayer.VMs;

namespace Project.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
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

        [HttpPost("addorder/{customerId}")]
        public async Task<IActionResult> AddOrder(Guid customerId, NewOrderVM newOrder)
        {
            try
            {
                await _orderService.CreateOrderAsync(newOrder, customerId);
                return Ok("Order added successfully");
            }
            catch (InvalidOperationException e)
            {
                return Conflict(new { Message = e.Message });
            }
            catch (KeyNotFoundException e) {
                return NotFound(new { Message = e.Message });
            }
            catch(NotFoundException e)
            {
                return NotFound(new { Message = e.Message });
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
