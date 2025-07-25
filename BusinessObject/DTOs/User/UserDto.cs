using DataAccess.Entities.Authorize;

namespace BusinessObject.DTOs.User
{
    public class UserDto
    {
        public required string Id { get; set; }
        public required string Email { get; set; }
        public string? FullName { get; set; }
        public Gender? Gender { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
