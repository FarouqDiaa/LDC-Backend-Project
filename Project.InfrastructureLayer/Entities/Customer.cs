using System.ComponentModel.DataAnnotations;


namespace Project.InfrastructureLayer.Entities
{
    public class Customer
    {
        [Key]
        public Guid CustomerId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, ErrorMessage = "Name shouldn't be over 20 characters")]
        public string Name { get; set; }

        public string Address { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [RegularExpression("Active|InActive", ErrorMessage = "Status must be either 'Active' or 'InActive'")]
        public string Status { get; set; } = "InActive";

        [Required(ErrorMessage = "Password Hash is required")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Password Salt is required")]
        public string PasswordSalt { get; set; }

        [Required]
        public bool IsAdmin { get; init; } = false;

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime UpdatedOn { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
