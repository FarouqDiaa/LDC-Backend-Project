
namespace Project.BusinessDomainLayer.DTOs
{
    public class CustomerDTO : BaseCustomerDTO
    {
        public Guid Id { get; set; }
        public bool IsAdmin { get; init; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }

        /// <summary>Order stats, populated by the admin listing.</summary>
        public int OrdersCount { get; set; }
        public double TotalSpent { get; set; }
    }
}
