using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BusinessDomainLayer.DTOs
{
    public class NewCustomerDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Name must be at least 4 characters and no more than 50 characters")]
        [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z]{4,}$", ErrorMessage = "Name must contain only letters and be at least 4 characters long")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters and no more than 100 characters")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Password must contain at least one letter and one number")]
        public string Password { get; set; }

        [StringLength(200, ErrorMessage = "Address shouldn't be over 200 characters")]
        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage = "Address can only contain letters, numbers, and spaces")]
        public string Address { get; set; }

        [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be 11 digits")]
        public string Phone { get; set; }
    }
}
