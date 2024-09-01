
namespace Project.BusinessDomainLayer.VMs
{
    public class OrderItemResVM : BaseOrderItemVM
    {
        public required Guid Id { get; set; }
        public decimal Cost { get; set; }

    }
}
