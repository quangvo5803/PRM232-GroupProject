using BusinessObject.DTOs.Authorize;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebClient.Services.Interface;

namespace WebClient.Controllers
{
    public class AuthorizeController : Controller
    {
        private readonly IApiService _apiService;

        public AuthorizeController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDto dto)
        {
            var response = await _apiService.PostAsync(
                "/api/Authorize/register",
                dto,
                isSkip: true
            );

            if (!response.IsSuccessStatusCode)
            {
                await HandleValidationErrorAsync(response);
                return View(dto);
            }

            TempData["Email"] = dto.Email;
            TempData["FullName"] = dto.FullName;
            return RedirectToAction("VerifyOtp");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Vui lòng điền đầy đủ thông tin.";
                return View(dto);
            }
            var response = await _apiService.PostAsync("/api/Authorize/login", dto, isSkip: true);

            if (!response.IsSuccessStatusCode)
            {
                await HandleValidationErrorAsync(response);
                return View(dto);
            }

            TempData["Email"] = dto.Email;
            return RedirectToAction("VerifyOtp");
        }

        [HttpGet]
        public IActionResult VerifyOtp()
        {
            var email = TempData["Email"]?.ToString();
            return View(new VerifyOTPRequestDto { Email = email ?? "", OTP = "" });
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOtp(VerifyOTPRequestDto dto)
        {
            var response = await _apiService.PostAsync(
                "/api/Authorize/verify-otp",
                dto,
                isSkip: true
            );

            if (!response.IsSuccessStatusCode)
            {
                await HandleValidationErrorAsync(response);
                return View(dto);
            }

            var token = await response.Content.ReadFromJsonAsync<TokenResponseDto>();
            if (token != null)
            {
                HttpContext.Session.SetString("UserId", token.UserId);
                HttpContext.Session.SetString("Email", token.Email);
                HttpContext.Session.SetString("Role", token.Role);
                HttpContext.Session.SetString("AccessToken", token.AccessToken);
                HttpContext.Session.SetString("RefreshToken", token.RefreshToken);
                HttpContext.Session.SetString(
                    "AccessTokenExpiresAt",
                    token.AccessTokenExpiresAt.ToString("O")
                );
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // Helper: Handle validation errors from WebAPI
        private async Task HandleValidationErrorAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            try
            {
                var errorObj = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                if (errorObj != null && errorObj.ContainsKey("errors"))
                {
                    var errors = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(
                        errorObj["errors"].ToString()
                    );

                    var errorMessages = new List<string>();
                    foreach (var kvp in errors)
                    {
                        foreach (var error in kvp.Value)
                        {
                            errorMessages.Add(error);
                        }
                    }

                    TempData["error"] = string.Join("<br/>", errorMessages);
                }
                else if (errorObj != null && errorObj.ContainsKey("message"))
                {
                    TempData["error"] = errorObj["message"].ToString();
                }
                else
                {
                    TempData["error"] = "Đã xảy ra lỗi không xác định.";
                }
            }
            catch
            {
                TempData["error"] = "Đã xảy ra lỗi không xác định.";
            }
        }
    }
}
