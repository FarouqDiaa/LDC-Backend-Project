using System.ComponentModel.DataAnnotations;


namespace Project.InfrastructureLayer.Entities
{
    public class Customer : BaseEntity
    {

        public string Name { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;

        public string Phone { get; set; } = String.Empty;

        public string Status { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public bool IsAdmin { get; init; } = false;


        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
