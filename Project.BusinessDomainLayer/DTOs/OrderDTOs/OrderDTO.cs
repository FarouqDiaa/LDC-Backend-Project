using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.BusinessDomainLayer.DTOs
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }

        public decimal Amount { get; set; }

        public decimal TotalAmount { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }

        public List<OrderItemDTO> OrderItems { get; set; } = [];

    }
}
