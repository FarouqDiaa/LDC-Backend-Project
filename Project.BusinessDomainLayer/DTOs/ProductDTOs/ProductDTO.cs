using System.ComponentModel.DataAnnotations;

namespace Project.BusinessDomainLayer.DTOs
{
    public class ProductDTO : BaseProductDTO
    {

        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
