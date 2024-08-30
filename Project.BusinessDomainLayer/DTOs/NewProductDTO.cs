
using System.ComponentModel.DataAnnotations;

namespace Project.BusinessDomainLayer.DTOs
{
    public class NewProductDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Name must be at least 4 characters and no more than 100 characters")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description shouldn't be over 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Type is required")]
        [StringLength(50, ErrorMessage = "Type shouldn't be over 50 characters")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be a positive value")]
        public double Cost { get; set; }

        [Required(ErrorMessage = "StockQuantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "StockQuantity must be a positive value")]
        public int StockQuantity { get; set; }
    }
}
