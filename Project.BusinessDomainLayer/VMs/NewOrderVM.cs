using Project.BusinessDomainLayer.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Project.BusinessDomainLayer.VMs
{
    public class NewOrderVM
    {
        [Range(0, float.MaxValue, ErrorMessage = "Tax must be a positive value")]
        public float Tax { get; set; }

        [Required]
        public List<NewOrderItemVM> OrderItems { get; set; } = new List<NewOrderItemVM>();
    }
}
