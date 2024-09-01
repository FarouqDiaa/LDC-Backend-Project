using System.ComponentModel.DataAnnotations;

namespace Project.BusinessDomainLayer.DTOs
{
    public class OrderItemDTO : BaseOrderItemDTO
    {
        public required Guid Id { get; set; }
        public decimal Cost { get; set; }

    }
}
