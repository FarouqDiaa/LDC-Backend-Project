using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.InfrastructureLayer.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Type { get; set; }

        public string Status { get; set; }

        public double Cost { get; set; }

        public int StockQuantity { get; set; }

        public string? Sku { get; set; }

        // Pricing
        public string? DiscountType { get; set; }
        public double DiscountPercentage { get; set; }
        public string? TaxClass { get; set; }
        public double VatAmount { get; set; }

        // Shipping
        public bool IsPhysical { get; set; } = true;
        public string? Weight { get; set; }
        public string? Height { get; set; }
        public string? Length { get; set; }
        public string? Width { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    }
}
