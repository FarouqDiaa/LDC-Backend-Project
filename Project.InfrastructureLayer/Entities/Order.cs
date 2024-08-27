using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.InfrastructureLayer.Entities
{
    public class Order
    {
        public Guid OrderId { get; set; }


        public double Amount { get; set; }

        public float Tax { get; set; }

        public double TotalAmount { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsDeleted { get; set; } = false;

        public Guid CustomerId { get; set; }

        public Customer Customer { get; set; }


        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
