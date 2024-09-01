
namespace Project.BusinessDomainLayer.DTOs
{
    public class CustomerDTO : BaseCustomerDTO
    {
        public Guid Id { get; set; }
        public bool IsAdmin { get; init; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
