using BusinessObject.DTOs.User;

namespace BusinessObject.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> GetUserProfileAsync(string userId);
        Task<bool> UpdateUserProfileAsync(UserDto updateDto);
    }
}
