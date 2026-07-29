using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Project.BusinessDomainLayer.Abstractions;
using Project.BusinessDomainLayer.DTOs;
using Project.BusinessDomainLayer.Responses;

namespace Project.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IDashboardService dashboardService, ILogger<DashboardController> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;
        }

        // One call that returns everything the dashboard page renders.
        [HttpGet]
        public Task<IActionResult> GetOverview(
            [FromQuery] int salesDays = 30,
            [FromQuery] int topCount = 5,
            [FromQuery] int lowStockThreshold = 10,
            [FromQuery] int lowStockCount = 5)
        {
            return Execute(
                () => _dashboardService.GetOverviewAsync(salesDays, topCount, lowStockThreshold, lowStockCount),
                "Dashboard Retrieved Successfully",
                "Can’t Retrieve Dashboard");
        }

        [HttpGet("summary")]
        public Task<IActionResult> GetSummary() =>
            Execute(_dashboardService.GetSummaryAsync, "Summary Retrieved Successfully", "Can’t Retrieve Summary");

        [HttpGet("sales")]
        public Task<IActionResult> GetSalesOverTime([FromQuery] int days = 30) =>
            Execute(() => _dashboardService.GetSalesOverTimeAsync(days), "Sales Retrieved Successfully", "Can’t Retrieve Sales");

        [HttpGet("sales-by-category")]
        public Task<IActionResult> GetSalesByCategory() =>
            Execute(_dashboardService.GetSalesByCategoryAsync, "Categories Retrieved Successfully", "Can’t Retrieve Categories");

        [HttpGet("top-products")]
        public Task<IActionResult> GetTopProducts([FromQuery] int count = 5) =>
            Execute(() => _dashboardService.GetTopProductsAsync(count), "Top Products Retrieved Successfully", "Can’t Retrieve Top Products");

        [HttpGet("low-stock")]
        public Task<IActionResult> GetLowStock([FromQuery] int threshold = 10, [FromQuery] int count = 5) =>
            Execute(() => _dashboardService.GetLowStockAsync(threshold, count), "Low Stock Retrieved Successfully", "Can’t Retrieve Low Stock");

        // Shared success/error envelope so every endpoint responds identically.
        private async Task<IActionResult> Execute<T>(Func<Task<T>> action, string successMessage, string errorMessage)
            where T : class
        {
            try
            {
                var data = await action();
                return Ok(new SuccessResponse<T>
                {
                    StatusCode = 200,
                    Message = successMessage,
                    Data = data
                });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update exception caught in dashboard controller");
                return BadRequest(new ErrorResponse { StatusCode = 400, Message = errorMessage });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL exception caught in dashboard controller");
                return BadRequest(new ErrorResponse { StatusCode = 400, Message = errorMessage });
            }
        }
    }
}
