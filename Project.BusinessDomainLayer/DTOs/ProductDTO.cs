using System.ComponentModel.DataAnnotations;

namespace Project.BusinessDomainLayer.DTOs
{
    public class ProductDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name shouldn't be over 100 characters")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description shouldn't be over 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Type is required")]
        [StringLength(50, ErrorMessage = "Type shouldn't be over 50 characters")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [RegularExpression("Active|InActive", ErrorMessage = "Status must be either 'Active' or 'InActive'")]
        public string Status { get; set; } = "InActive";

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be a positive value.")]
        public double Amount { get; set; }

        public bool IsDeleted { get; set; }
    }
}
