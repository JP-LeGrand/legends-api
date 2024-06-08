using legend.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace legend.Entities
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int StockQuantity { get; set; }
        public FragranceType Type { get; set; }
        public GenderType Gender { get; set; }
        public PerfumeSize SizeInMilliliters { get; set; }
        public string Notes { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
