using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.DbContext;
using Shared.DTOs.SeedUser;
using System.Text.Json;

namespace Persistence.Seeds
{
    public class SedingData(ApplicationDbContext _dbContext,
        UserManager<ApplicationUser> _userManager,
        RoleManager<IdentityRole> _roleManager) : IDataSeed
    {
        public async Task DatSeedAsync()
        {
            try
            {
                var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                    await _dbContext.Database.MigrateAsync();

                                if (!_dbContext.Set<ApplicationUser>().Any())
                {
                    var roles = new List<string> { "Admin", "Instructor", "Student" };
                    foreach (var role in roles)
                    {
                                                if (!await _roleManager.RoleExistsAsync(role))
                            await _roleManager.CreateAsync(new IdentityRole(role));
                    }

                    var basePath = AppContext.BaseDirectory;
                    var fullPath = Path.Combine(basePath, "Seeds", "Data", "users.json");

                    if (File.Exists(fullPath))
                    {
                                                using var stream = File.OpenRead(fullPath);
                        var usersToSeed = await JsonSerializer.DeserializeAsync<List<SeedUserDto>>(stream);

                        if (usersToSeed != null)
                        {
                            foreach (var userDto in usersToSeed)
                            {
                                var user = new ApplicationUser
                                {
                                    FirstName = userDto.FirstName,
                                    LastName = userDto.LastName,
                                    UserName = userDto.UserName,
                                    Email = userDto.Email,
                                    EmailConfirmed = true
                                };
                                var result = await _userManager.CreateAsync(user, "Password123!");
                                if (result.Succeeded)
                                {
                                    await _userManager.AddToRoleAsync(user, userDto.Role);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during database seeding: {ex.Message}");
            }
        }
    }
}