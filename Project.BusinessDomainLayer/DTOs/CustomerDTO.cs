
namespace Project.BusinessDomainLayer.DTOs
{
    public class CustomerDTO
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        public bool IsAdmin { get; init; }
        public bool IsDeleted { get; set; }
    }
}
