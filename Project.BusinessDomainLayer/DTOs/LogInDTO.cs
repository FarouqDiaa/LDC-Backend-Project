using System.ComponentModel.DataAnnotations;

namespace Project.BusinessDomainLayer.DTOs
{
    public class LogInDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Email or Password is incorrect")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Email or Password is incorrect")]
        public string Password { get; set; }
    }
}
