using BusinessObject.DTOs.Authorize;
using DataAccess.Entities.Authorize;

namespace BusinessObject.Services.Interfaces
{
    public interface IAuthorizeService
    {
        Task SendRegisterOtpAsync(string email, string fullName);
        Task SendLoginOtpAsync(string email);
        Task<TokenResponseDto> VerifyOtpAsync(string email, string otp);
        Task<TokenResponseDto> RefreshTokenAsync(string refreshToken);
        Task<string> GenerateToken(ApplicationUser user);
        string GenerateRefeshToken();
    }
}
