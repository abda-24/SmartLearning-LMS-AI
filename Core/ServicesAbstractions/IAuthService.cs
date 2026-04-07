using Application.DTOs.Auth;
using Shared.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstractions
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto model);
        Task<AuthResponseDto> LoginAsync(LoginDto model);
        Task<string> ForgotPasswordAsync(ForgotPasswordDto model);
        Task<string> ResetPasswordAsync(ResetPasswordDto model);
        Task<AuthResponseDto> RefreshTokenAsync(TokenRequestDto model);
    }
}
