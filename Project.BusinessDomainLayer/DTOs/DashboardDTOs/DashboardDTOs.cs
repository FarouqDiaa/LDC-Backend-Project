namespace Project.BusinessDomainLayer.DTOs
{
    // Top summary numbers for the KPI cards.
    public class DashboardSummaryDTO
    {
        public double TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalProducts { get; set; }
        public double AverageOrderValue { get; set; }
    }

    // A single point on the sales line chart.
    public class SalesPointDTO
    {
        public DateTime Date { get; set; }
        public double Revenue { get; set; }
        public int Orders { get; set; }
    }

    // One slice of the sales-by-category pie chart.
    public class CategorySalesDTO
    {
        public string Type { get; set; } = string.Empty;
        public double Revenue { get; set; }
        public int UnitsSold { get; set; }
    }

    // A row in the best-sellers list.
    public class TopProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Type { get; set; }
        public int UnitsSold { get; set; }
        public double Revenue { get; set; }
        public string? ImageUrl { get; set; }
    }

    // A row in the low-stock alert list.
    public class LowStockProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Type { get; set; }
        public int StockQuantity { get; set; }
        public string? ImageUrl { get; set; }
    }

    // Everything the dashboard page needs, in one payload.
    public class DashboardOverviewDTO
    {
        public DashboardSummaryDTO Summary { get; set; } = new();
        public IEnumerable<SalesPointDTO> SalesOverTime { get; set; } = new List<SalesPointDTO>();
        public IEnumerable<CategorySalesDTO> SalesByCategory { get; set; } = new List<CategorySalesDTO>();
        public IEnumerable<TopProductDTO> TopProducts { get; set; } = new List<TopProductDTO>();
        public IEnumerable<LowStockProductDTO> LowStock { get; set; } = new List<LowStockProductDTO>();
    }
}
