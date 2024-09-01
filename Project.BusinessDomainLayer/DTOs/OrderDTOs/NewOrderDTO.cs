using Project.BusinessDomainLayer.VMs;
using System.ComponentModel.DataAnnotations;

namespace Project.BusinessDomainLayer.DTOs
{
    public class NewOrderDTO : BaseOrderDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool IsDeleted { get; set; } = false;
        public decimal Amount { get; set; } = 0; 
        public decimal TotalAmount { get; set; } = 0;
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [Required]
        public List<NewOrderItemDTO> OrderItems { get; set; } = [];
    }
}
