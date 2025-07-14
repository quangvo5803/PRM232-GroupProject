using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities.Application
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }

        [Range(1, 5)]
        public int FeedbackStars { get; set; }
        public string? FeedbackContent { get; set; }

        public Guid UserId { get; set; }

        public int ProductId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual ICollection<ItemImage>? Images { get; set; }
    }
}
