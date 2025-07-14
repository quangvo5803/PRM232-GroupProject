using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities.Application
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
