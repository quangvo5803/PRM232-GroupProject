using System.ComponentModel.DataAnnotations;
using BusinessObject.DTOs.Product;

namespace BusinessObject.DTOs.OrderDetail
{
    public class OrderDetailDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public double UnitPrice { get; set; }
        public double Total => Quantity * UnitPrice;
        public ProductDto? Product { get; set; }
    }
}
