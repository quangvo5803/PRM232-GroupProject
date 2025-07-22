namespace BusinessObject.DTOs.ShoppingCart
{
    public class ShoppingCartDTO
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public double Price { get; set; }

        public int Count { get; set; }

        public Guid UserId { get; set; }
    }
}
