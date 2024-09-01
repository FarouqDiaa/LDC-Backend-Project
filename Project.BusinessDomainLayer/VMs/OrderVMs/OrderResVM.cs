
namespace Project.BusinessDomainLayer.VMs
{
    public class OrderResVM
    {
        public required Guid Id { get; set; }
        public required Guid CustomerId { get; set; }

        public decimal Tax { get; set; }
        public bool IsDeleted { get; set; }

        public decimal Amount { get; set; }

        public decimal TotalAmount { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }

        public List<OrderItemResVM> OrderItems { get; set; }
    }
}
