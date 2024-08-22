using System.ComponentModel.DataAnnotations;

namespace Project.BusinessDomainLayer.DTOs
{
    public class OrderDTO
    {
        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be a positive value")]
        public double Amount { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Tax must be a positive value")]
        public float Tax { get; set; }

        [Required(ErrorMessage = "Total Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total Amount must be a positive value")]
        public double TotalAmount { get; set; }

        public bool IsDeleted { get; set; }
    }
}
