using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.InfrastructureLayer.Entities
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid CustomerId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The Amount cannot be negative")]
        public double Amount { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "The Tax cannot be negative")]
        public float Tax { get; set; }

        public double TotalAmount { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedOn { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
