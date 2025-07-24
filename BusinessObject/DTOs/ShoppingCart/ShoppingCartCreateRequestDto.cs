namespace BusinessObject.DTOs.ShoppingCart
{
    public class ShoppingCartCreateRequestDto
    {
        public int ProductId { get; set; }

        public int Quantity{ get; set; }

        public Guid UserId { get; set; }
    }
}
