using System.Diagnostics;
using BusinessObject.DTOs.Product;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;
using WebClient.Services.Interface;

namespace WebClient.Controllers;

public class HomeController : Controller
{
    private IApiService _apiService;

    public HomeController(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        var response = await _apiService.GetAsync("/api/Customer/GetAllProducts");
        if (!response.IsSuccessStatusCode)
        {
            TempData["error"] = "Failed to load products.";
            return View();
        }
        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        return View(products);
    }

    public async Task<IActionResult> Menu()
    {
        var response = await _apiService.GetAsync("/api/Customer/GetAllProducts");
        if (!response.IsSuccessStatusCode)
        {
            TempData["error"] = "Failed to load products.";
            return View();
        }
        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        return View(products);
    }

    public async Task<IActionResult> ProductDetail(int id, int pageNumber = 1, int pageSize = 5)
    {
        var response = await _apiService.GetAsync($"/api/Customer/GetProductById/{id}");
        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandler.HandleValidationErrorAsync(response, TempData);
            return RedirectToAction("Index");
        }
        var product = await response.Content.ReadFromJsonAsync<ProductDto>();

        //var feedbacks = _unitOfWork.Feedback.GetRange(
        //    f => f.ProductId == id,
        //    includeProperties: "User,Images"
        //);
        //if (product != null && product.ProductImages != null)
        //{
        //    product.ProductImages = product
        //        .ProductImages.Where(img => img.FeedbackId == null)
        //        .ToList();
        //}
        //int totalFeedbacks = feedbacks.Count();

        //var feedbackPage = feedbacks.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        //if (product == null)
        //{
        //    TempData["error"] = "Error! Cannot load product data";
        //    return RedirectToAction("Index", "Home");
        //}
        //ViewBag.Feedbacks = feedbacks;
        //ViewBag.PageNumber = pageNumber;
        //ViewBag.PageSize = pageSize;
        return View(product);
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
