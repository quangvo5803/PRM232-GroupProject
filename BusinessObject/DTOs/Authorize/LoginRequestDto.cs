using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTOs.Authorize
{
    public class LoginRequestDto
    {
        [Required, EmailAddress]
        public required string Email { get; set; }
    }
}
