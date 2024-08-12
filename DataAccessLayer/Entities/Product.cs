using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDCBackendProject.Core.Entities
{
    public class Product 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public int stockQuantity { get; set; }
        public string imageURL { get; set; }
        public bool isRemoved { get; set; }
    }
}
