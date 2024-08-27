using System.ComponentModel.DataAnnotations;


namespace Project.InfrastructureLayer.Entities
{
    public class Customer
    {
        public Guid CustomerId { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;

        public string Phone { get; set; } = String.Empty;

        public string Status { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public bool IsAdmin { get; init; } = false;

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsDeleted { get; set; } = false;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
