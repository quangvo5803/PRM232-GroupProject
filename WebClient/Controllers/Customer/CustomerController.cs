using BusinessObject.DTOs.Orders;
using BusinessObject.DTOs.User;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Utilities.Exceptions;

namespace WebClient.Controllers.Customer
{
    public partial class CustomerController : Controller
    {
        public async Task<IActionResult> Profile()
        {
            var token = HttpContext.Session.GetString("AccessToken");

            var userId = HttpContext.Session.GetString("UserId");
            var email = HttpContext.Session.GetString("Email");
            if (
                string.IsNullOrEmpty(token)
                || string.IsNullOrEmpty(userId)
                || string.IsNullOrEmpty(email)
            )
            {
                return RedirectToAction("Login", "Authorize");
            }
            var response = await _apiService.GetAsync(
                $"/api/Customer/GetUserProfile/{userId}",
                isSkip: false
            );

            if (!response.IsSuccessStatusCode)
            {
                await ErrorHandler.HandleValidationErrorAsync(response, TempData);
                return View(new UserDto { Id = userId, Email = email });
            }

            var content = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserDto>(content);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserDto dto)
        {
            var token = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Authorize");
            }

            var response = await _apiService.PutAsync(
                "/api/Customer/UpdateUserProfile",
                dto,
                isSkip: false
            );

            if (!response.IsSuccessStatusCode)
            {
                await ErrorHandler.HandleValidationErrorAsync(response, TempData);
                return RedirectToAction("Profile");
            }

            TempData["Success"] = "Cập nhật thông tin thành công!";
            return RedirectToAction("Profile");
        }

        public async Task<IActionResult> OrderHistory()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Authorize");
            }

            var response = await _apiService.GetAsync(
                $"/api/Customer/OrderHistory/{userId}",
                false
            );

            if (!response.IsSuccessStatusCode)
            {
                TempData["error"] = "Không thể tải lịch sử đơn hàng.";
                return View(new List<OrderDto>());
            }

            var content = await response.Content.ReadAsStringAsync();
            var orders = JsonConvert.DeserializeObject<List<OrderDto>>(content);

            return View(orders);
        }

        public async Task<IActionResult> CancelOrder(int id)
        {
            var response = await _apiService.DeleteAsync($"/api/Customer/CancelOrder/{id}", isSkip: false);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Order canceled successfully.";
            }
            else
            {
                TempData["error"] = "Failed to cancel the order.";
            }

            return RedirectToAction("OrderHistory");
        }
    }
}
