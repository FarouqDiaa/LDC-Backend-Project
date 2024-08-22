using System.ComponentModel.DataAnnotations;

namespace Project.BusinessDomainLayer.DTOs
{
    public class NewCustomerDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, ErrorMessage = "Name shouldn't be over 20 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters and no more than 100 characters")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Password must contain at least one letter and one number")]
        public string Password { get; set; }

        [StringLength(200, ErrorMessage = "Address shouldn't be over 200 characters")]
        public string Address { get; set; }

        [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be 11 digits")]
        public string Phone { get; set; }
    }
}
