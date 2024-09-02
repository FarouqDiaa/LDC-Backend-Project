
using Project.BusinessDomainLayer.VMs;
using System.ComponentModel.DataAnnotations;

namespace Project.BusinessDomainLayer.DTOs
{
    public class BaseOrderDTO
    {

        [Required(ErrorMessage = "Customer Id is required")]
        public required Guid CustomerId { get; set; }
    }
}
