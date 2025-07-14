using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities.Application
{
    public class ItemImage
    {
        [Key]
        public Guid ImageID { get; set; }

        public required string ImageUrl { get; set; }
        public required int ProductId { get; set; }
        public int? FeedbackId { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [ForeignKey("FeedbackId")]
        public Feedback? Feedback { get; set; }
        public string PublicId { get; set; }
    }
}
