using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.InfrastructureLayer.Entities
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100, ErrorMessage = "The Name field cannot exceed 100 characters")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "The Description field cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(50, ErrorMessage = "The Type field cannot exceed 50 characters")]
        public string Type { get; set; }

        [Required]
        [RegularExpression("OutOfStock|InStock", ErrorMessage = "Status must be either 'OutOfStock' or 'InStock'")]
        public string Status { get; set; } = "InStock";

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be a positive value")]
        [Column(TypeName = "decimal(18,2)")]
        public double Cost { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock Quantity cannot be negative")]
        public int StockQuantity { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime UpdatedOn { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
