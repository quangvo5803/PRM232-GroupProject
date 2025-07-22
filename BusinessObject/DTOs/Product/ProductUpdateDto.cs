using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BusinessObject.DTOs.Product
{
    public class ProductUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public string? OldAvatar { get; set; }
        public List<string>? OldImages { get; set; }
        public IFormFile? ProductAvatar { get; set; }
        public List<IFormFile>? ProductImages { get; set; }
    }
}
