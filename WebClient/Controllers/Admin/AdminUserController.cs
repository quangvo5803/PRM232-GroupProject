using System.Threading.Tasks;
using BusinessObject.DTOs.Product;
using BusinessObject.DTOs.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace WebClient.Controllers.Admin
{
    public partial class AdminController : Controller
    {
        public IActionResult CustomerList()
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
        public async Task<IActionResult> GetAllCustomer()
        {
            var response = await _apiService.GetAsync("/api/Admin/GetAllCustomers", isSkip: false);
            if (!response.IsSuccessStatusCode)
            {
                await ErrorHandler.HandleValidationErrorAsync(response, TempData);
                return Json(new { data = new List<UserDto>() });
            }
            var customersResponse =
                await response.Content.ReadFromJsonAsync<List<UserDto>>() ?? new List<UserDto>();
            var customers = customersResponse
                .Select(c => new
                {
                    c.Id,
                    c.Email,
                    c.FullName,
                    c.PhoneNumber,
                })
                .ToList();
            return Json(new { data = customers });
        }

        [HttpGet]
        public async Task<IActionResult> CustomerDetail(Guid id)
        {
            var response = await _apiService.GetAsync(
                $"/api/Admin/GetUserProfile/{id}",
                isSkip: false
            );
            if (!response.IsSuccessStatusCode)
            {
                await ErrorHandler.HandleValidationErrorAsync(response, TempData);
                return RedirectToAction("Index");
            }
            var customer = await response.Content.ReadFromJsonAsync<UserDto>();
            if (customer == null)
            {
                TempData["error"] = "Customer not found.";
                return RedirectToAction("CustomerList");
            }

            return View(customer);
        }
    }
}
