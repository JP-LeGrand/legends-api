using legend.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace legend.Entities
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OrderId { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public ShippingDetails? shippingDetails { get; set; }

        // Navigation property for user
        public User? User { get; set; }

        // Collection navigation property for order items
        public List<OrderItem> OrderItems { get; set; }
        public OrderStatus Status { get; set; }
    }

}
