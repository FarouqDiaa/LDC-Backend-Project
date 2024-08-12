using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDCBackendProject.Core.Entities
{
    public class OrderProduct
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }
        public int Quantity { get; set; }
    }
}
