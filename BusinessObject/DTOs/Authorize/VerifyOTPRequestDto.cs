using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTOs.Authorize
{
    public class VerifyOTPRequestDto
    {
        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string OTP { get; set; }
    }
}
