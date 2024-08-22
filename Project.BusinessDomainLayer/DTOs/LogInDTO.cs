using System.ComponentModel.DataAnnotations;

namespace Project.BusinessDomainLayer.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(20, ErrorMessage = "Username shouldn't be over 20 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters and no more than 100 characters")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Password can only contain letters and numbers")]
        public string Password { get; set; }
    }
}
