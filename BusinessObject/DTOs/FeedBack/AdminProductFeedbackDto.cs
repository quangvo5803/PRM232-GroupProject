using DataAccess.Entities.Application;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs.FeedBack
{
    public class AdminProductFeedbackDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? ProductAvatar { get; set; }
        public List<FeedbackDto>? Feedbacks { get; set; }
    }
}
