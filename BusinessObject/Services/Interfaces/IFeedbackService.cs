

using BusinessObject.DTOs.FeedBack;

namespace BusinessObject.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task<List<FeedbackDto>> GetAllFeedbackAsync(int id);
        Task<FeedbackDto> CreateFeedbackAsync(FeedbackCreateRequestDto requestDto);
        Task<AdminProductFeedbackDto> GetFeedbackProductAdminAsync(int productId);
    }
}
