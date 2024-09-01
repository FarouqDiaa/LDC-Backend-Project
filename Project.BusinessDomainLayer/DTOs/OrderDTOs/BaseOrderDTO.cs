
using Project.BusinessDomainLayer.VMs;
using System.ComponentModel.DataAnnotations;

namespace Project.BusinessDomainLayer.DTOs
{
    public class BaseOrderDTO
    {

        [Required(ErrorMessage = "Customer Id is required")]
        public required Guid CustomerId { get; set; }

        [Range(0, 1, ErrorMessage = "Tax must be a value between 0 and 1")]
        public decimal Tax { get; set; } = 0.14m;
    }
}
