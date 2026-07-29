using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Project.BusinessDomainLayer.VMs.ProductVMs
{
    public class ProductVM : BaseProductVM
    {
        /// <summary>Url of the main image shown in listings.</summary>
        public string? CoverImageUrl { get; set; }

        /// <summary>The gallery images, excluding the cover.</summary>
        public List<ProductImageVM> ProductImages { get; set; } = new();
    }
}
