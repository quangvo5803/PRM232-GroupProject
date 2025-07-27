using BusinessObject.DTOs.Orders;
using BusinessObject.DTOs.Product;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace WebClient.Controllers.Admin
{
    public partial class AdminController : Controller
    {
        public IActionResult OrderList()
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");
            var userRole = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Authorize");
            }
            if (userRole != "Admin")
            {
                TempData["error"] = "You do not have permission to access this page.";
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var response = await _apiService.GetAsync("/api/Admin/GetAllOrders", isSkip: false);
            if (!response.IsSuccessStatusCode)
            {
                await ErrorHandler.HandleValidationErrorAsync(response, TempData);
                return Json(new { data = new List<OrderDto>() });
            }
            var ordersResponse =
                await response.Content.ReadFromJsonAsync<List<OrderDto>>() ?? new List<OrderDto>();
            var orders = ordersResponse
                .Select(o => new
                {
                    o.Id,
                    o.UserFullName,
                    o.TotalPrice,
                    o.OrderDate,
                    o.Status,
                })
                .ToList();
            return Json(new { data = orders });
        }
    }
}
