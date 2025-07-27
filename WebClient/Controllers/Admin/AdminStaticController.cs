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
        public async Task<IActionResult> GetStatistic(int? year = null)
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

            try
            {
                var response = await _apiService.GetAsync("/api/Admin/Statistic", isSkip: false);


                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    await ErrorHandler.HandleValidationErrorAsync(response, TempData);
                    return View("Index", new StaticDto());
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                var staticData = await response.Content.ReadFromJsonAsync<StaticDto>();

                if (staticData == null)
                {
                    TempData["error"] = "Data not found.";
                    return View("Index", new StaticDto());
                }


                var years = new List<SelectListItem>();
                var currentYear = DateTime.Now.Year;
                var selectedYear = year ?? currentYear;

                for (int i = currentYear - 4; i <= currentYear; i++)
                {
                    years.Add(new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString(),
                        Selected = i == selectedYear
                    });
                }
                ViewBag.Years = years;
                ViewBag.SelectedYear = selectedYear;

                return View("Index", staticData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Frontend Error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                TempData["error"] = "An error occurred while loading data.";
                return View("Index", new StaticDto());
            }
        }

    }
}
