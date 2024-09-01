
using System.ComponentModel.DataAnnotations;

namespace Project.BusinessDomainLayer.VMs
{
    public class BaseCustomerVM
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be at least 2 characters and no more than 50 characters")]
        [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z]{2,}$", ErrorMessage = "Name must contain only letters")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(300, ErrorMessage = "Address shouldn't be over 300 characters")]
        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage = "Address can only contain letters, numbers, and spaces")]
        public required string Address { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^\+?\d{11,14}$", ErrorMessage = "Phone number must be between 11 and 14 digits, with an optional '+' sign at the beginning")]
        public required string Phone { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [RegularExpression(@"^(Active|InActive)$", ErrorMessage = "Status must be either 'Active' or 'InActive'")]
        public required string Status { get; set; }
    }
}
