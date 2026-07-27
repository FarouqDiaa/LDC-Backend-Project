using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Project.BusinessDomainLayer.DTOs
{
    public class ProductDTO : BaseProductDTO
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }

        public string? CoverImageUrl { get; set; }

        public List<ProductImageDTO> ProductImages { get; set; } = new();
    }
}
