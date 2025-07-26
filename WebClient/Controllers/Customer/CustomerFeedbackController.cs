using BusinessObject.DTOs.FeedBack;
using BusinessObject.DTOs.Orders;
using DataAccess.Entities.Application;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace WebClient.Controllers.Customer
{
    public partial class CustomerController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> SubmitFeedBack(FeedbackCreateRequestDto requestDto)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out Guid userId))
            {
                TempData["error"] = "Session expired or invalid.";
                return RedirectToAction("Login", "Authorize");
            }
            requestDto.UserId = userId;

            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(requestDto.FeedbackStars.ToString()), "feedbackStars");
            formData.Add(new StringContent(requestDto.FeedbackContent ?? ""), "feedbackContent");
            formData.Add(new StringContent(requestDto.UserId.ToString()), "userId");
            formData.Add(new StringContent(requestDto.ProductId.ToString()), "productId");

            if (requestDto.Images != null && requestDto.Images.Any())
            {
                foreach (var img in requestDto.Images)
                {
                    var imgContent = new StreamContent(img.OpenReadStream());
                    imgContent.Headers.ContentType = new MediaTypeHeaderValue(img.ContentType);
                    formData.Add(imgContent, "Images", img.FileName);
                }
            }
            var response = await _apiService.PostAsync(
                "/api/Customer/CreateFeedback",
                formData,
                isSkip: false
            );

            if (!response.IsSuccessStatusCode)
            {
                TempData["error"] = "Feedback order failed.";
                return RedirectToAction("OrderHistory");
            }

            TempData["success"] = "Feedback successfully.";
            return RedirectToAction("OrderHistory", "Customer");
        }
    }
}
