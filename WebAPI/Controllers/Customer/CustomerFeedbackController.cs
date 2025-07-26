using BusinessObject.DTOs.FeedBack;
using BusinessObject.DTOs.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Customer
{
    public partial class CustomerController : ControllerBase
    {
        [HttpGet("GetAllFeedback/{id}")]
        public async Task<IActionResult> GetAllFeedback(int id)
        {
            var rs = await _facadeService.Feedback.GetAllFeedbackAsync(id);
            return Ok(rs);
        }

        [HttpPost("CreateFeedback")]
        public async Task<IActionResult> CreateFeedback([FromForm] FeedbackCreateRequestDto requestDto)
        {
            if (requestDto == null)
            {
                return BadRequest("Feedback data is required.");
            }
            var feedBack = await _facadeService.Feedback.CreateFeedbackAsync(requestDto);
            return Ok(feedBack);
        }
    }
}
