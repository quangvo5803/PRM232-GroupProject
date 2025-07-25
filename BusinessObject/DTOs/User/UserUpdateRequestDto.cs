using DataAccess.Entities.Authorize;

namespace BusinessObject.DTOs.User
{
    public class UserUpdateRequestDto
    {
        public string? FullName { get; set; }
        public Gender? Gender { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
