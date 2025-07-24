using BusinessObject.DTOs.Category;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;
using WebClient.Services.Interface;

namespace WebClient.Controllers.Admin
{
    public partial class AdminController : Controller
    {
        private readonly IApiService _apiService;

        public AdminController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index()
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

        public IActionResult CategoryList()
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
        public async Task<IActionResult> GetAllCategories()
        {
            var response = await _apiService.GetAsync("/api/Admin/GetAllCategories", isSkip: false);
            if (!response.IsSuccessStatusCode)
            {
                await ErrorHandler.HandleValidationErrorAsync(response, TempData);
                return Json(new { data = new List<CategoryDto>() });
            }

            var categories = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
            return Json(new { data = categories });
        }

        [HttpGet]
        public IActionResult CreateCategory()
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

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryCreateRequestDto category)
        {
            var response = await _apiService.PostAsync(
                "/api/Admin/CreateCategory",
                category,
                isSkip: false
            );
            if (!response.IsSuccessStatusCode)
            {
                await ErrorHandler.HandleValidationErrorAsync(response, TempData);
                return View();
            }
            TempData["success"] = "Category added successfully.";
            return RedirectToAction("CategoryList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
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
            var response = await _apiService.GetAsync(
                $"/api/Admin/GetCategoryById/{id}",
                isSkip: false
            );
            if (!response.IsSuccessStatusCode)
            {
                await ErrorHandler.HandleValidationErrorAsync(response, TempData);
                return RedirectToAction("CategoryList");
            }
            var category = await response.Content.ReadFromJsonAsync<CategoryDto>();
            if (category == null)
            {
                TempData["error"] = "Category does not exist.";
                return RedirectToAction("CategoryList");
            }
            var categoryUpdateRequest = new CategoryUpdateRequestDto
            {
                Id = category.Id,
                Name = category.Name,
            };
            return View(categoryUpdateRequest);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(CategoryUpdateRequestDto category)
        {
            var response = await _apiService.PutAsync(
                "/api/Admin/UpdateCategory",
                category,
                isSkip: false
            );
            if (!response.IsSuccessStatusCode)
            {
                await ErrorHandler.HandleValidationErrorAsync(response, TempData);
                return View();
            }
            TempData["success"] = "Catalog updated successfully.";
            return RedirectToAction("CategoryList");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var response = await _apiService.DeleteAsync(
                $"/api/Admin/DeleteCategory/{id}",
                isSkip: false
            );
            if (!response.IsSuccessStatusCode)
            {
                await ErrorHandler.HandleValidationErrorAsync(response, TempData);
                return Json(new { success = false, message = "Delete category failed." });
            }
            TempData["success"] = "Category deletion successful.";
            return Json(new { success = true, message = "Category deletion successful." });
        }
    }
}
