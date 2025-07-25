using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities.Application
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

    }
}
