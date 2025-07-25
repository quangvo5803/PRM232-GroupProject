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
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Authorize");
            }

            var response = await _apiService.GetAsync(
                "/api/User/Get",
                isSkip: false
            );

            if (!response.IsSuccessStatusCode)
            {
                await ErrorHandler.HandleValidationErrorAsync(response, TempData);
                return View(new UserDto());
            }

            var content = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserDto>(content);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserUpdateRequestDto dto)
        {
            var token = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Authorize");
            }

            var response = await _apiService.PutAsync(
                "/api/User/Update",
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
    }
}
