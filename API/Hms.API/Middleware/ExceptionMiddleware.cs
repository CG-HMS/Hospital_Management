using System.Net;
using System.Text.Json;
using Hms.API.Exceptions;
using System.Text.Json;

namespace Hms.API.Middleware
{
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
            _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
            catch (Exception ex)
        {
                _logger.LogError(ex, "Exception: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex.StatusCode;

            int statusCode;
            string message;
            string details;

            switch (ex)
            {
                case AppException appEx:
                    statusCode = appEx.StatusCode;
                    message = appEx.Message;
                    details = ex.GetType().Name;
                    break;

                case FluentValidation.ValidationException valEx:
                    statusCode = 400;
                    message = "One or more validation errors occurred.";
                    details = string.Join(" | ", valEx.Errors.Select(e => e.ErrorMessage));
                    break;

                default:
                    statusCode = 500;
                    message = "An unexpected error occurred. Please try again later.";
                    details = "InternalServerError";
                    break;
        }

            context.Response.StatusCode = statusCode;

            var body = new
            {
                statusCode,
                message,
                details,
                timestamp = DateTime.UtcNow
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(body,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
        }
    }
}