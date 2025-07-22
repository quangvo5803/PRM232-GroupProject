using System.ComponentModel.DataAnnotations;
using BusinessObject.DTOs.Category;

namespace BusinessObject.DTOs.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public double Price { get; set; }
        public CategoryDto Category { get; set; }
        public string? ProductAvatar { get; set; }
        public List<string> ProductImages { get; set; } = new List<string>();
    }
}
