using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.InfrastructureLayer.Entities
{
    public class OrderItem
    {
        [Key]
        public Guid OrderitemId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cost must be a positive value")]
        [Column(TypeName = "decimal(18,2)")]
        public double Cost { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
