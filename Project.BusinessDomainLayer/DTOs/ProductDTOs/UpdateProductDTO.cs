
namespace Project.BusinessDomainLayer.DTOs
{
    public class UpdateProductDTO : BaseProductDTO
    {
        public string? CoverImageUrl { get; set; }

        public List<ProductImageDTO> ProductImages { get; set; } = new();
    }
}
