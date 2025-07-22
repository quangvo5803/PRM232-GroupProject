using BusinessObject.DTOs.Authorize;
using BusinessObject.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly IAuthorizeService _authorizeService;

        public AuthorizeController(IAuthorizeService authorizeService)
        {
            _authorizeService = authorizeService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            await _authorizeService.SendRegisterOtpAsync(dto.Email, dto.FullName);
            return Ok(new { message = "OTP đã được gửi đến email của bạn" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            await _authorizeService.SendLoginOtpAsync(dto.Email);
            return Ok(new { message = "OTP đã được gửi đến email của bạn" });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOTPRequestDto dto)
        {
            var tokens = await _authorizeService.VerifyOtpAsync(dto.Email, dto.OTP);
            return Ok(tokens);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto dto)
        {
            var tokens = await _authorizeService.RefreshTokenAsync(dto.RefreshToken);
            return Ok(tokens);
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequestDto dto)
        {
            var result = await _authorizeService.LoginWithGoogleAsync(dto.IdToken);
            return Ok(result);
        }
    }
}
