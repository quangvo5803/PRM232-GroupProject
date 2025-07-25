using BusinessObject.DTOs.OrderDetail;
using DataAccess.Entities.Application;
using System.ComponentModel.DataAnnotations;



namespace BusinessObject.DTOs.Orders
{
    public class OrderCreateRequestDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public required string PaymentMethod { get; set; } // "PayByCash" hoặc "VNPay"

        public string? ShippingAddress { get; set; }
        public string? PhoneNumber { get; set; }

        public List<OrderDetailCreateRequestDto> OrderDetails { get; set; } = new();   
    }
}
