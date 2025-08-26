using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Project.BusinessDomainLayer.VMs.ProductVMs
{
    public class ProductVM : BaseProductVM
    {
        public List<ProductImageVM> ProductImages { get; set; } = new();
    }
}
