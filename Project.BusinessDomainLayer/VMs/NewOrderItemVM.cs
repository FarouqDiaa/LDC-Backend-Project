using System.ComponentModel.DataAnnotations;

namespace Project.BusinessDomainLayer.VMs
{
    public class NewOrderItemVM
    {
        [Required(ErrorMessage = "Added Product Id is required")]
        public Guid ProductId { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}
