using BusinessObject.DTOs.ShoppingCart;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebClient.Services.Interface;

namespace WebClient.Controllers.Customer
{
    public class CustomerController : Controller
    {
        private readonly IApiService _apiService;

        public CustomerController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Authorize");
            }

            var response = await _apiService.GetAsync($"/api/CustomerCart/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Không thể tải giỏ hàng.";
                return View(new List<ShoppingCartDTO>());
            }

            var data = await response.Content.ReadAsStringAsync();
            var cartItems = JsonConvert.DeserializeObject<List<ShoppingCartDTO>>(data);
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] ShoppingCartCreateRequestDto cartDto)
        {
            var response = await _apiService.PostAsync("/api/CustomerCart/AddItem", cartDto);
            return Json(new { success = response.IsSuccessStatusCode });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateItem([FromBody] ShoppingCartUpdateRequestDto cartDto)
        {
            var response = await _apiService.PutAsync("/api/CustomerCart/UpdateItem", cartDto);
            return Json(new { success = response.IsSuccessStatusCode });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var response = await _apiService.DeleteAsync($"/api/CustomerCart/DeleteItem/{id}");
            return Json(new { success = response.IsSuccessStatusCode });
        }
    }
}
