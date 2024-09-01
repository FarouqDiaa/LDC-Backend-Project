using System.ComponentModel.DataAnnotations;

namespace Project.BusinessDomainLayer.VMs
{
    public class OrderVM : BaseOrderVM
    {
        [Required]
        public List<OrderItemVM> OrderItems { get; set; } = [];
    }
}
