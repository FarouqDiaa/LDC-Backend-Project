using System.ComponentModel.DataAnnotations;

namespace Project.BusinessDomainLayer.DTOs
{
    public class NewOrderDTO
    {
        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be a positive value")]
        public double Amount { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Tax must be a positive value")]
        public float Tax { get; set; }
    }
}
