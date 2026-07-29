
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

        [StringLength(50, ErrorMessage = "SKU shouldn't be over 50 characters")]
        public string? Sku { get; set; }

        // Pricing
        public string? DiscountType { get; set; }

        [Range(0, 100, ErrorMessage = "Discount percentage must be between 0 and 100")]
        public double DiscountPercentage { get; set; }

        public string? TaxClass { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "VAT amount must be a positive value")]
        public double VatAmount { get; set; }

        // Shipping
        public bool IsPhysical { get; set; } = true;
        public string? Weight { get; set; }
        public string? Height { get; set; }
        public string? Length { get; set; }
        public string? Width { get; set; }
    }
}
