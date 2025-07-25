using BusinessObject.DTOs.ShoppingCart;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Utilities.Exceptions;
using WebClient.Services.Interface;

namespace WebClient.Controllers.Customer
{
    public partial class CustomerController : Controller
    {
        private readonly IApiService _apiService;

        public CustomerController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Cart()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Authorize");
            }

            var response = await _apiService.GetAsync(
                $"/api/Customer/GetCart/{userId}",
                isSkip: false
            );
            if (!response.IsSuccessStatusCode)
            {
                await ErrorHandler.HandleValidationErrorAsync(response, TempData);
                return View(new List<ShoppingCartDTO>());
            }

            var data = await response.Content.ReadAsStringAsync();
            var cartItems = JsonConvert.DeserializeObject<List<ShoppingCartDTO>>(data);
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] ShoppingCartCreateRequestDto cartDto)
        {
            var response = await _apiService.PostAsync(
                "/api/Customer/AddItem",
                cartDto,
                isSkip: false
            );
            return Json(new { success = response.IsSuccessStatusCode });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateItem([FromBody] ShoppingCartUpdateRequestDto cartDto)
        {
            var response = await _apiService.PutAsync(
                "/api/Customer/UpdateItem",
                cartDto,
                isSkip: false
            );
            return Json(new { success = response.IsSuccessStatusCode });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var response = await _apiService.DeleteAsync(
                $"/api/Customer/DeleteItem/{id}",
                isSkip: false
            );
            return Json(new { success = response.IsSuccessStatusCode });
        }
    }
}
