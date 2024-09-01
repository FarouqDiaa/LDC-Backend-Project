
using System.ComponentModel.DataAnnotations;

namespace Project.BusinessDomainLayer.VMs
{
    public class BaseProductVM
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name shouldn't be over 50 characters")]
        public required string Name { get; set; }

        [StringLength(150, ErrorMessage = "Description shouldn't be over 150 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be a positive value")]
        public required double Amount { get; set; }

        [Required(ErrorMessage = "Type is required")]
        [StringLength(50, ErrorMessage = "Type shouldn't be over 50 characters")]
        public required string Type { get; set; }


        private int _stockQuantity;

        [Range(0, int.MaxValue, ErrorMessage = "StockQuantity must be a positive value")]
        public int StockQuantity
        {
            get => _stockQuantity;
            set
            {
                if (_status.Equals("OutOfStock", StringComparison.OrdinalIgnoreCase))
                {
                    _stockQuantity = 0;
                }
                else
                {
                    _stockQuantity = value > 0 ? value : 100;
                }
            }
        }

        private string _status = string.Empty;

        [Required(ErrorMessage = "Status is required")]
        [RegularExpression(@"^(InStock|OutOfStock)$", ErrorMessage = "Status must be either 'InStock' or 'OutOfStock'")]
        public string Status
        {
            get => _status;
            set
            {
                _status = value;

                if (_status.Equals("InStock", StringComparison.OrdinalIgnoreCase) && _stockQuantity == 0)
                {
                    _stockQuantity = 100;
                }
                else if (_status.Equals("OutOfStock", StringComparison.OrdinalIgnoreCase))
                {
                    _stockQuantity = 0;
                }
            }
        }

    }
}
