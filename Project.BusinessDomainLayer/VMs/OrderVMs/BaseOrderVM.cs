
using System.ComponentModel.DataAnnotations;

namespace Project.BusinessDomainLayer.VMs
{
    public class BaseOrderVM
    {
        [Required(ErrorMessage = "Customer Id is required")]
        public required Guid CustomerId { get; set; }

    }
}
