using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities.Application
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public int Count { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
