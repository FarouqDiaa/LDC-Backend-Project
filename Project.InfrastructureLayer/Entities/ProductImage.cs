
namespace Project.InfrastructureLayer.Entities
{
    public class ProductImage : BaseEntity
    {
        public Guid ProductimageId { get; set; }
        public string Url { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
