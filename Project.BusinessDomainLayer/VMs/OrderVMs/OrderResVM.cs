
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

        // Shipping details captured at checkout.
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Notes { get; set; }

        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }

        public DateTime UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }

        public List<OrderItemResVM> OrderItems { get; set; }
    }
}
