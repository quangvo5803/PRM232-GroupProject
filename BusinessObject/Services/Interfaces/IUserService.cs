using BusinessObject.DTOs.User;

namespace BusinessObject.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllCustomersAsync();
        Task<UserDto?> GetUserProfileAsync(string userId);
        Task<bool> UpdateUserProfileAsync(UserDto updateDto);
    }
}
