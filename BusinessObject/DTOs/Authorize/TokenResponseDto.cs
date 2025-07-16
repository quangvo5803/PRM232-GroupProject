namespace BusinessObject.DTOs.Authorize
{
    public class TokenResponseDto
    {
        public required string UserId { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public DateTime AccessTokenExpiresAt { get; set; }
    }
}
