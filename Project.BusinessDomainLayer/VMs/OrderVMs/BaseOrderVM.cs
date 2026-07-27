
using System.ComponentModel.DataAnnotations;

namespace Project.BusinessDomainLayer.VMs
{
    public class BaseOrderVM
    {
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Full name shouldn't be over 100 characters")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required")]
        [StringLength(20, ErrorMessage = "Phone shouldn't be over 20 characters")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [StringLength(250, ErrorMessage = "Address shouldn't be over 250 characters")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required")]
        [StringLength(100, ErrorMessage = "City shouldn't be over 100 characters")]
        public string City { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Notes shouldn't be over 500 characters")]
        public string? Notes { get; set; }
    }
}
