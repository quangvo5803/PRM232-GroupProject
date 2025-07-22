using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BusinessObject.DTOs.Product
{
    public class ProductCreateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? ProductAvatar { get; set; }
        public List<IFormFile>? ProductImages { get; set; }
    }
}
