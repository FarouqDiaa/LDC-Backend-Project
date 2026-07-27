
using Project.BusinessDomainLayer.VMs;
using System.ComponentModel.DataAnnotations;

namespace Project.BusinessDomainLayer.DTOs
{
    public class BaseOrderDTO
    {

        [Required(ErrorMessage = "Customer Id is required")]
        public required Guid CustomerId { get; set; }

        // Shipping details captured at checkout.
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Notes { get; set; }
    }
}
