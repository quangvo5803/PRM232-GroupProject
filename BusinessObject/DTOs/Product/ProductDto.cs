using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTOs.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public int? ProductAvatarId { get; set; }
    }
} 