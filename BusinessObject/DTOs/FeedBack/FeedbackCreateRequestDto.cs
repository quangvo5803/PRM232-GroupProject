using Microsoft.AspNetCore.Http;


namespace BusinessObject.DTOs.FeedBack
{
    public class FeedbackCreateRequestDto
    {
        public int FeedbackStars { get; set; }
        public string? FeedbackContent { get; set; }
        public Guid UserId { get; set; }
        public int ProductId { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}
