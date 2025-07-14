using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities.Application
{
    public class ItemImage
    {
        [Key]
        public Guid ImageID { get; set; }

        public required string ImageUrl { get; set; }
        public required int ProductId { get; set; }
        public string PublicId { get; set; }
    }
}
