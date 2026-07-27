
namespace Project.BusinessDomainLayer.VMs
{
    public class CustomerResVM : BaseCustomerVM
    {
        public required Guid Id { get; set; }
        public bool IsAdmin { get; init; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string? Token { get; set; }

        public int OrdersCount { get; set; }
        public double TotalSpent { get; set; }
    }
}
