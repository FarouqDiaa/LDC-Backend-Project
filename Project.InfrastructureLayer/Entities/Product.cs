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

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
