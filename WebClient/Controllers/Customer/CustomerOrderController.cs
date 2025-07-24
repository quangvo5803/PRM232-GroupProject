using Azure;
using BusinessObject.DTOs.Orders;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
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
        public async Task<IActionResult> CheckOut()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Authorize");
            }
            var response = await _apiService.GetAsync($"/api/Customer/checkout/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["error"] = "Không thể tải giỏ hàng.";
                return View(new List<CheckOutDto>());
            }

            var data = await response.Content.ReadAsStringAsync();
            var cartItems = JsonConvert.DeserializeObject<List<CheckOutDto>>(data);
            ViewBag.TotalPrice = cartItems!.Sum(x => x.Price * x.Count);
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(OrderCreateRequestDto orderDto)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid input data.";
                return RedirectToAction("CheckOut");
            }

            var response = await _apiService.PostAsync("/api/Customer/createorder", orderDto, isSkip:false);

            if (!response.IsSuccessStatusCode)
            {
                TempData["error"] = "Tạo đơn hàng thất bại.";
                return RedirectToAction("CheckOut");
            }

            if (orderDto.PaymentMethod == "VNPay")
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(responseBody);

                string paymentUrl = result!.PaymentUrl;
                return Redirect(paymentUrl); // Điều hướng đến trang VNPay
            }

            TempData["success"] = "Payment successfully.";
            // Nếu là thanh toán tiền mặt thì chuyển về trang cảm ơn đơn giản
            return RedirectToAction("Profile");
        }

    }
}
