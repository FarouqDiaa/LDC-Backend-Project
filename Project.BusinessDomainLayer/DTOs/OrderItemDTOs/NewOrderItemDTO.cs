
namespace Project.BusinessDomainLayer.DTOs
{
    public class NewOrderItemDTO : BaseOrderItemDTO
    {
        public required Guid Id { get; set; } = Guid.NewGuid();
        public decimal Cost { get; set; } = 0;

    }
}
