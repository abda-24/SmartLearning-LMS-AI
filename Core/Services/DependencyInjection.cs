using Application.Services;
using Application.Services.CourseSer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Ai;
using Services.CourseSer;
using Services.UploadFiles;
using Services.UserProfile;
using ServicesAbstractions;
using StackExchange.Redis;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
        {
           
            services.AddAutoMapper(options =>
            {
                options.AddProfile<CourseProfile>();
            });
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IProgressService, ProgressService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IUserService, UserService>();
           services.AddScoped<IAIService, GeminiService>();
            services.AddSingleton<IConnectionMultiplexer>(
           ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis") ?? "localhost:6379")
);

            services.AddScoped<ICacheService, CacheService>();
            services.AddHttpClient<IAiBrainService, Infrastructure.AI.GeminiBrainService>();
            services.AddHttpClient<IFawaterakPaymentService, Infrastructure.Services.FawaterakPaymentService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IWishlistService, WishlistService>();
            return services;
        }
    }
}