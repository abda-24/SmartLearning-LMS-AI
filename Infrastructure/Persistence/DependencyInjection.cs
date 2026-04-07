using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using Persistence.DbContext;
using Persistence.Repositories;
using Persistence.Seeds;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDataSeed, SedingData>();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMailKit(mailConfig =>
            {
                mailConfig.UseMailKit(new MailKitOptions()
                {
                    Server = configuration["MailSettings:Host"],
                    Port = int.Parse(configuration["MailSettings:Port"]!),
                    SenderName = configuration["MailSettings:DisplayName"],
                    SenderEmail = configuration["MailSettings:Email"],
                    Account = configuration["MailSettings:Email"],
                    Password = configuration["MailSettings:Password"],
                    Security = true
                });
            });

            services.AddScoped<Microsoft.EntityFrameworkCore.DbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            return services;
        }
    }
}