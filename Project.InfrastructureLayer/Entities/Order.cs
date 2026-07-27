using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.InfrastructureLayer.Entities
{
    public class Order : BaseEntity
    {
        public double Amount { get; set; }

        public float Tax { get; set; }

        public double TotalAmount { get; set; }

        // Shipping details captured at checkout.
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Notes { get; set; }

        public Guid CustomerId { get; set; }

        public Customer Customer { get; set; }


        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
