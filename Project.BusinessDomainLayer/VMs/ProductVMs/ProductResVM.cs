
using Project.BusinessDomainLayer.VMs.ProductVMs;
using System.ComponentModel.DataAnnotations;

namespace Project.BusinessDomainLayer.VMs
{
    public class ProductResVM : BaseProductVM
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        /// <summary>Cover image url, also present in <see cref="ProductImages"/>.</summary>
        public string? CoverImageUrl { get; set; }

        public List<ProductImageVM> ProductImages { get; set; } = new();
    }
}
