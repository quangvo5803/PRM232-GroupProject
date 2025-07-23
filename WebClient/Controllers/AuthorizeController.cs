using BusinessObject.DTOs.Authorize;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;
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
                await ErrorHandler.HandleValidationErrorAsync(response, TempData);
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
                await ErrorHandler.HandleValidationErrorAsync(response, TempData);
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
                await ErrorHandler.HandleValidationErrorAsync(response, TempData);
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

            switch (token?.Role)
            {
                case "Admin":
                    return RedirectToAction("Index", "Admin");
                case "Customer":
                    return RedirectToAction("Index", "Home");
                case "Restaurent":
                    return RedirectToAction("Index", "Restaurent");
                default:
                    TempData["error"] = "No access.";
                    return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequestDto dto)
        {
            var response = await _apiService.PostAsync("/api/Authorize/google-login", dto);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(new { success = false, message = "Google login failed" });
            }

            var token = await response.Content.ReadFromJsonAsync<TokenResponseDto>();
            if (token == null)
            {
                return BadRequest(new { success = false, message = "Invalid token" });
            }

            HttpContext.Session.SetString("UserId", token.UserId);
            HttpContext.Session.SetString("Email", token.Email);
            HttpContext.Session.SetString("Role", token.Role);
            HttpContext.Session.SetString("AccessToken", token.AccessToken);
            HttpContext.Session.SetString("RefreshToken", token.RefreshToken);
            HttpContext.Session.SetString(
                "AccessTokenExpiresAt",
                token.AccessTokenExpiresAt.ToString("O")
            );

            return Ok(new { success = true, role = token.Role });
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
