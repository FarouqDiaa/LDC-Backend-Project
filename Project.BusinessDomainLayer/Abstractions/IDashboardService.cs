using Project.BusinessDomainLayer.DTOs;

namespace Project.BusinessDomainLayer.Abstractions
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDTO> GetSummaryAsync();
        Task<IEnumerable<SalesPointDTO>> GetSalesOverTimeAsync(int days);
        Task<IEnumerable<CategorySalesDTO>> GetSalesByCategoryAsync();
        Task<IEnumerable<TopProductDTO>> GetTopProductsAsync(int count);
        Task<IEnumerable<LowStockProductDTO>> GetLowStockAsync(int threshold, int count);
        Task<DashboardOverviewDTO> GetOverviewAsync(int salesDays, int topCount, int lowStockThreshold, int lowStockCount);
    }
}
