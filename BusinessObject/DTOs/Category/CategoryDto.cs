using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTOs.Category
{
    public class CategoryDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
    }
}
