using BusinessObject.DTOs.OrderDetail;

namespace BusinessObject.DTOs.Orders
{
    public class OrderDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }
        public required string PaymentMethod { get; set; }
        public required string Status { get; set; }
        public string? ShippingAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UserFullName { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; } = new();
    }
}
