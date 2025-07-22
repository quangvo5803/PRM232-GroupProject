using System.Net.Http.Headers;
using System.Threading.Tasks;
using BusinessObject.DTOs.Category;
using BusinessObject.DTOs.Product;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace WebClient.Controllers.Admin
{
    public partial class AdminController : Controller
    {
        public IActionResult ProductList()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            var response = await _apiService.GetAsync("/api/Admin/GetAllProducts", isSkip: false);
            if (!response.IsSuccessStatusCode)
            {
                await ErrorHandler.HandleValidationErrorAsync(response, TempData);
                return Json(new { data = new List<ProductDto>() });
            }
            var productsResponse =
                await response.Content.ReadFromJsonAsync<List<ProductDto>>()
                ?? new List<ProductDto>();
            var products = productsResponse
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    CategoryName = p.Category?.Name,
                })
                .ToList();
            return Json(new { data = products });
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            var response = await _apiService.GetAsync("/api/Admin/GetAllCategories", isSkip: false);
            if (!response.IsSuccessStatusCode)
            {
                await ErrorHandler.HandleValidationErrorAsync(response, TempData);
                return Json(new { data = new List<CategoryDto>() });
            }
            ViewBag.Categories = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateDto product)
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(product.Name), "Name");
            formData.Add(new StringContent(product.Price.ToString()), "Price");
            formData.Add(new StringContent(product.Description ?? ""), "Description");
            formData.Add(new StringContent(product.CategoryId.ToString()), "CategoryId");

            if (product.ProductAvatar != null)
            {
                var avatarContent = new StreamContent(product.ProductAvatar.OpenReadStream());
                avatarContent.Headers.ContentType = new MediaTypeHeaderValue(
                    product.ProductAvatar.ContentType
                );
                formData.Add(avatarContent, "ProductAvatar", product.ProductAvatar.FileName);
            }

            if (product.ProductImages != null && product.ProductImages.Any())
            {
                foreach (var img in product.ProductImages)
                {
                    var imgContent = new StreamContent(img.OpenReadStream());
                    imgContent.Headers.ContentType = new MediaTypeHeaderValue(img.ContentType);
                    formData.Add(imgContent, "ProductImages", img.FileName);
                }
            }

            var response = await _apiService.PostAsync("/api/Admin/CreateProduct", formData);
            if (!response.IsSuccessStatusCode)
            {
                await ErrorHandler.HandleValidationErrorAsync(response, TempData);
                response = await _apiService.GetAsync("/api/Admin/GetAllCategories", isSkip: false);
                ViewBag.Categories = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
                return View();
            }
            TempData["success"] = "Product added successfully.";
            return RedirectToAction("ProductList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var response = await _apiService.GetAsync(
                $"/api/Admin/GetProductById/{id}",
                isSkip: false
            );
            if (!response.IsSuccessStatusCode)
            {
                await ErrorHandler.HandleValidationErrorAsync(response, TempData);
                return RedirectToAction("ProductList");
            }
            var product = await response.Content.ReadFromJsonAsync<ProductDto>();
            if (product == null)
            {
                TempData["error"] = "Product not found.";
                return RedirectToAction("ProductList");
            }
            response = await _apiService.GetAsync("/api/Admin/GetAllCategories", isSkip: false);
            if (!response.IsSuccessStatusCode)
            {
                await ErrorHandler.HandleValidationErrorAsync(response, TempData);
                return RedirectToAction("ProductList");
            }
            var updateProduct = new ProductUpdateDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CategoryId = product.Category?.Id ?? 0,
                OldAvatar = product.ProductAvatar,
                OldImages = product.ProductImages,
            };
            ViewBag.Categories = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
            return View(updateProduct);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(ProductUpdateDto product)
        {
            var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(product.Id.ToString()), "Id");
            formData.Add(new StringContent(product.Name), "Name");
            formData.Add(new StringContent(product.Price.ToString()), "Price");
            formData.Add(new StringContent(product.Description ?? ""), "Description");
            formData.Add(new StringContent(product.CategoryId.ToString()), "CategoryId");

            if (product.ProductAvatar != null)
            {
                var avatarContent = new StreamContent(product.ProductAvatar.OpenReadStream());
                avatarContent.Headers.ContentType = new MediaTypeHeaderValue(
                    product.ProductAvatar.ContentType
                );
                formData.Add(avatarContent, "ProductAvatar", product.ProductAvatar.FileName);
            }

            if (product.ProductImages != null && product.ProductImages.Any())
            {
                foreach (var img in product.ProductImages)
                {
                    var imgContent = new StreamContent(img.OpenReadStream());
                    imgContent.Headers.ContentType = new MediaTypeHeaderValue(img.ContentType);
                    formData.Add(imgContent, "ProductImages", img.FileName);
                }
            }

            var response = await _apiService.PutAsync("/api/Admin/UpdateProduct", formData);

            if (!response.IsSuccessStatusCode)
            {
                await ErrorHandler.HandleValidationErrorAsync(response, TempData);

                var cateRes = await _apiService.GetAsync(
                    "/api/Admin/GetAllCategories",
                    isSkip: false
                );
                ViewBag.Categories = await cateRes.Content.ReadFromJsonAsync<List<CategoryDto>>();

                return View();
            }

            TempData["success"] = "Product updated successfully.";
            return RedirectToAction("ProductList");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var response = await _apiService.DeleteAsync(
                $"/api/Admin/DeleteProduct/{id}",
                isSkip: false
            );
            if (!response.IsSuccessStatusCode)
            {
                await ErrorHandler.HandleValidationErrorAsync(response, TempData);
                return Json(new { success = false, message = "Failed to delete product." });
            }
            TempData["success"] = "Product deleted successfully.";
            return Json(new { success = true, message = "Product deleted successfully." });
        }
    }
}
