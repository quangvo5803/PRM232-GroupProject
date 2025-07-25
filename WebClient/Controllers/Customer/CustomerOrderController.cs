using System.Net.Http;
using System.Text;
using Azure;
using BusinessObject.DTOs.Orders;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebClient.Controllers.Customer
{
    public partial class CustomerController : Controller
    {
        public async Task<IActionResult> CheckOut()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Authorize");
            }
            var response = await _apiService.GetAsync(
                $"/api/Customer/Checkout/{userId}",
                isSkip: false
            );

            if (!response.IsSuccessStatusCode)
            {
                TempData["error"] = "Unable to load cart.";
                return View(new List<CheckOutDto>());
            }

            var data = await response.Content.ReadAsStringAsync();
            var cartItems = JsonConvert.DeserializeObject<List<CheckOutDto>>(data);
            ViewBag.TotalPrice = cartItems!.Sum(x => x.Price * x.Count);
            ViewBag.Email = email;
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(OrderCreateRequestDto orderDto)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out Guid userId))
            {
                TempData["error"] = "Session expired or invalid.";
                return RedirectToAction("Login", "Authorize");
            }
            orderDto.UserId = userId;
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid input data.";
                return RedirectToAction("CheckOut");
            }

            var response = await _apiService.PostAsync(
                "/api/Customer/CreateOrder",
                orderDto,
                isSkip: false
            );

            if (!response.IsSuccessStatusCode)
            {
                TempData["error"] = "Order creation failed.";
                return RedirectToAction("CheckOut");
            }

            if (orderDto.PaymentMethod == "VNPay")
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<JObject>(responseBody);

                string? paymentUrl = (string?)result["paymentUrl"];

                return Redirect(paymentUrl!);
            }

            TempData["success"] = "Payment successfully.";
            return RedirectToAction("Index", "Home");
        }
    }
}
