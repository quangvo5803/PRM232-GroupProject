using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTOs.Authorize
{
    public class RegisterRequestDto
    {
        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string FullName { get; set; }
    }
}
