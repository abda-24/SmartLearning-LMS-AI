using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using ServicesAbstractions;
using Shared.DTOs.Users;

namespace Services.UserProfile
{
    public class UserService(UserManager<ApplicationUser> _userManager) : IUserService
    {
        public async Task<UserProfileDto?> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            return new UserProfileDto
            {
                Id = user.Id,
                Email = user.Email!,
                UserName = user.UserName!,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<bool> UpdateUserProfileAsync(string userId, UpdateProfileDto updateDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.PhoneNumber = updateDto.PhoneNumber;
            user.FirstName = updateDto.FirstName;
            user.LastName = updateDto.LastName;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}