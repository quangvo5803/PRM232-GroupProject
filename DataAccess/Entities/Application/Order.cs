﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    }

    public enum OrderStatus
    {
        Pending,
        Completed,
        Cancelled,
    }
}
