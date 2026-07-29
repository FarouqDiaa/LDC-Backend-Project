namespace Project.InfrastructureLayer.Abstractions
{
    // Read-only aggregation queries that power the admin dashboard. These sit
    // apart from the entity repositories because they never return tracked
    // entities — only pre-shaped tuples for the stats widgets.
    public interface IDashboardRepository
    {
        Task<double> GetTotalRevenueAsync();
        Task<int> GetTotalOrdersAsync();
        Task<int> GetTotalCustomersAsync();
        Task<int> GetTotalProductsAsync();

        /// <summary>Daily revenue and order counts for the last <paramref name="days"/> days.</summary>
        Task<IEnumerable<(DateTime Date, double Revenue, int Orders)>> GetSalesOverTimeAsync(int days);

        Task<IEnumerable<(string Type, double Revenue, int UnitsSold)>> GetSalesByCategoryAsync();

        Task<IEnumerable<(Guid Id, string Name, string? Type, int UnitsSold, double Revenue, string? ImageUrl)>> GetTopProductsAsync(int count);

        Task<IEnumerable<(Guid Id, string Name, string? Type, int StockQuantity, string? ImageUrl)>> GetLowStockProductsAsync(int threshold, int count);
    }
}
