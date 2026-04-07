using Application.DTOs.Auth;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Core;
using ServicesAbstractions;
using Shared.DTOs.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        IEmailService emailService,
        IServiceProvider serviceProvider,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _emailService = emailService;
        _serviceProvider = serviceProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto model)
    {
        if (await _userManager.FindByEmailAsync(model.Email) != null)
            return new AuthResponseDto { IsSuccess = false, Message = "Email already exists" };

        var user = new ApplicationUser
        {
            UserName = string.IsNullOrEmpty(model.UserName) ? model.Email : model.UserName,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return new AuthResponseDto
            {
                IsSuccess = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            };
        }

        var roleToAssign = string.IsNullOrEmpty(model.Role) ? "Student" : model.Role;

        if (!await _roleManager.RoleExistsAsync(roleToAssign))
            await _roleManager.CreateAsync(new IdentityRole(roleToAssign));

        await _userManager.AddToRoleAsync(user, roleToAssign);

        if (roleToAssign == "Instructor")
        {
            var instructor = new Instructor
            {
                UserId = user.Id,
                Bio = "New Instructor",
                Specialty = "General",
                Rating = 0
            };

            await _unitOfWork.Repository<Instructor, int>().AddAsync(instructor);
            await _unitOfWork.CompleteAsync();
        }

        return new AuthResponseDto
        {
            IsSuccess = true,
            Message = "User registered successfully"
        };
    }
    public async Task<AuthResponseDto> LoginAsync(LoginDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            return new AuthResponseDto { IsSuccess = false, Message = "Invalid credentials" };

        var jwtToken = await GenerateJwtTokenAsync(user);
        var refreshToken = new RefreshToken
        {
            Token = GenerateRefreshTokenString(),
            ExpiresOn = DateTime.UtcNow.AddDays(7),
            CreatedOn = DateTime.UtcNow
        };

        user.RefreshTokens.Add(refreshToken);
        await _userManager.UpdateAsync(user);

        return new AuthResponseDto
        {
            IsSuccess = true,
            Message = "Login successful",
            Token = jwtToken,
            RefreshToken = refreshToken.Token,
            RefreshTokenExpiration = refreshToken.ExpiresOn
        };
    }

    public async Task<string> ForgotPasswordAsync(ForgotPasswordDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) return "Reset link sent if email exists";

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var resetLink = $"{_configuration["FrontendUrl"]}/reset-password?email={model.Email}&token={encodedToken}";

        var emailBody = $"<h3>Reset Password</h3><p>Please reset your password by <a href='{resetLink}'>clicking here</a>.</p>";
        await _emailService.SendAsync(user.Email, "Reset Password", emailBody, true);

        return "Reset link sent if email exists";
    }

    public async Task<string> ResetPasswordAsync(ResetPasswordDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) return "Invalid request";

        try
        {
            var originalToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
            var result = await _userManager.ResetPasswordAsync(user, originalToken, model.NewPassword);

            return result.Succeeded ? "Password reset successfully" : "Reset failed: " + string.Join(", ", result.Errors.Select(e => e.Description));
        }
        catch
        {
            return "Invalid token format";
        }
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(TokenRequestDto model)
    {
        var principal = GetPrincipalFromExpiredToken(model.AccessToken);
        if (principal == null) return new AuthResponseDto { IsSuccess = false, Message = "Invalid access token" };

        var email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var user = await _userManager.Users.Include(u => u.RefreshTokens).SingleOrDefaultAsync(u => u.Email == email);

        if (user == null) return new AuthResponseDto { IsSuccess = false, Message = "User not found" };

        var activeToken = user.RefreshTokens.FirstOrDefault(t => t.Token == model.RefreshToken);
        if (activeToken == null || !activeToken.IsActive)
            return new AuthResponseDto { IsSuccess = false, Message = "Invalid or expired refresh token" };

        activeToken.RevokedOn = DateTime.UtcNow;

        var newJwt = await GenerateJwtTokenAsync(user);
        var newRefreshToken = new RefreshToken
        {
            Token = GenerateRefreshTokenString(),
            ExpiresOn = DateTime.UtcNow.AddDays(7),
            CreatedOn = DateTime.UtcNow
        };

        user.RefreshTokens.Add(newRefreshToken);
        await _userManager.UpdateAsync(user);

        return new AuthResponseDto
        {
            IsSuccess = true,
            Token = newJwt,
            RefreshToken = newRefreshToken.Token,
            RefreshTokenExpiration = newRefreshToken.ExpiresOn
        };
    }

    private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.UserName!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
            claims.Add(new Claim("role", role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddHours(3),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshTokenString()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        var parameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, parameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            return null;

        return principal;
    }
}