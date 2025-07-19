using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTOs.Category
{
    public class CategoryCreateRequestDto
    {
        public required string Name { get; set; }
    }
}
