using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Auth
{
    public class TokenRequestDto
    {
        [Required(ErrorMessage = "Access token is required")]
        public string AccessToken { get; set; } = string.Empty;

        [Required(ErrorMessage = "Refresh token is required")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}