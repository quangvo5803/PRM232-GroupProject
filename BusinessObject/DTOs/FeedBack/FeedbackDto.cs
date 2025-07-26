

namespace BusinessObject.DTOs.FeedBack
{
    public class FeedbackDto
    {
        public int FeedbackStars { get; set; }
        public string? FeedbackContent { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public int ProductId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string>? Images { get; set; }
    }
}
