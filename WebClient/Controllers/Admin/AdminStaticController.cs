using BusinessObject.DTOs.Admin;
using BusinessObject.DTOs.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Utilities.Exceptions;

namespace WebClient.Controllers.Admin
{
    public partial class AdminController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetStatistic()
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

            var response = await _apiService.GetAsync("/api/Admin/Statistic", isSkip: false);
            if (!response.IsSuccessStatusCode)
            {
                await ErrorHandler.HandleValidationErrorAsync(response, TempData);
                return View(new StaticDto()); 
            }

            var staticData = await response.Content.ReadFromJsonAsync<StaticDto>();
            if (staticData == null)
            {
                TempData["error"] = "Data not found.";
                return View(new StaticDto());
            }

            var years = new List<SelectListItem>();
            var currentYear = DateTime.Now.Year;
            for (int i = currentYear - 4; i <= currentYear; i++)
            {
                years.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
            }

            ViewBag.Years = years;
            ViewBag.SelectedYear = currentYear;

            return View(staticData);
        }

    }
}
