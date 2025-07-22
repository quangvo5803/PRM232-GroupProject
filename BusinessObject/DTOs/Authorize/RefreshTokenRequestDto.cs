using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTOs.Authorize
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public required string RefreshToken { get; set; }
    }
}
