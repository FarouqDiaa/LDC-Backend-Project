using Project.BusinessDomainLayer.Abstractions;
using Project.BusinessDomainLayer.DTOs;
using Project.InfrastructureLayer.Abstractions;

namespace Project.BusinessDomainLayer.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        private const int MaxSalesDays = 365;
        private const int DefaultSalesDays = 30;
        private const int MaxListCount = 50;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        private static int Clamp(int value, int fallback, int max) =>
            value <= 0 || value > max ? fallback : value;

        public async Task<DashboardSummaryDTO> GetSummaryAsync()
        {
            var revenue = await _dashboardRepository.GetTotalRevenueAsync();
            var orders = await _dashboardRepository.GetTotalOrdersAsync();
            var customers = await _dashboardRepository.GetTotalCustomersAsync();
            var products = await _dashboardRepository.GetTotalProductsAsync();

            return new DashboardSummaryDTO
            {
                TotalRevenue = Math.Round(revenue, 2),
                TotalOrders = orders,
                TotalCustomers = customers,
                TotalProducts = products,
                AverageOrderValue = orders > 0 ? Math.Round(revenue / orders, 2) : 0d
            };
        }

        public async Task<IEnumerable<SalesPointDTO>> GetSalesOverTimeAsync(int days)
        {
            days = Clamp(days, DefaultSalesDays, MaxSalesDays);
            var rows = await _dashboardRepository.GetSalesOverTimeAsync(days);
            return rows.Select(r => new SalesPointDTO
            {
                Date = r.Date,
                Revenue = Math.Round(r.Revenue, 2),
                Orders = r.Orders
            });
        }

        public async Task<IEnumerable<CategorySalesDTO>> GetSalesByCategoryAsync()
        {
            var rows = await _dashboardRepository.GetSalesByCategoryAsync();
            return rows.Select(r => new CategorySalesDTO
            {
                Type = r.Type,
                Revenue = Math.Round(r.Revenue, 2),
                UnitsSold = r.UnitsSold
            });
        }

        public async Task<IEnumerable<TopProductDTO>> GetTopProductsAsync(int count)
        {
            count = Clamp(count, 5, MaxListCount);
            var rows = await _dashboardRepository.GetTopProductsAsync(count);
            return rows.Select(r => new TopProductDTO
            {
                Id = r.Id,
                Name = r.Name,
                Type = r.Type,
                UnitsSold = r.UnitsSold,
                Revenue = Math.Round(r.Revenue, 2),
                ImageUrl = r.ImageUrl
            });
        }

        public async Task<IEnumerable<LowStockProductDTO>> GetLowStockAsync(int threshold, int count)
        {
            threshold = threshold < 0 ? 10 : threshold;
            count = Clamp(count, 5, MaxListCount);
            var rows = await _dashboardRepository.GetLowStockProductsAsync(threshold, count);
            return rows.Select(r => new LowStockProductDTO
            {
                Id = r.Id,
                Name = r.Name,
                Type = r.Type,
                StockQuantity = r.StockQuantity,
                ImageUrl = r.ImageUrl
            });
        }

        public async Task<DashboardOverviewDTO> GetOverviewAsync(int salesDays, int topCount, int lowStockThreshold, int lowStockCount)
        {
            return new DashboardOverviewDTO
            {
                Summary = await GetSummaryAsync(),
                SalesOverTime = await GetSalesOverTimeAsync(salesDays),
                SalesByCategory = await GetSalesByCategoryAsync(),
                TopProducts = await GetTopProductsAsync(topCount),
                LowStock = await GetLowStockAsync(lowStockThreshold, lowStockCount)
            };
        }
    }
}
