namespace BusinessObject.DTOs.ShoppingCart
{
    public class ShoppingCartCreateRequestDto
    {
        public int ProductId { get; set; }

        public double Price { get; set; }

        public int Count { get; set; }

        public Guid UserId { get; set; }
    }
}
