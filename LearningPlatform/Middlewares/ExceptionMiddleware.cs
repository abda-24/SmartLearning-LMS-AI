using System.Net;
using System.Text.Json;

namespace LearningPlatform.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext _context)
        {
            try
            {
                await _next(_context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                _context.Response.ContentType = "application/json";
                _context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var actualError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                var response = _env.IsDevelopment()
                    ? new ApiException(_context.Response.StatusCode, actualError)
                    : new ApiException(_context.Response.StatusCode, "Internal Server Error");

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);

                await _context.Response.WriteAsync(json);
            }
        }
    }
}