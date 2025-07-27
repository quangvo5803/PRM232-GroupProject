using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Entities.Authorize;

namespace DataAccess.Entities.Application
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public double TotalPrice { get; set; }

        [Required]
        public string PaymentMethod { get; set; } // "PayByCash" hoặc "VNPay"

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public string? ShippingAddress { get; set; }
        public string? PhoneNumber { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }

    public enum OrderStatus
    {
        Pending,
        Completed,
        Cancelled,
    }

    public enum PaymentMethod
    {
        PayByCash,
        VNPay,
    }
}
