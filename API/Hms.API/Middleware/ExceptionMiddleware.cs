// Middleware/ExceptionMiddleware.cs  — single merged global exception handler
using Hms.API.Exceptions;
using System.Net;
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
                _logger.LogError(ex, "Unhandled exception on {Method} {Path}: {Message}",
                    context.Request.Method, context.Request.Path, ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            int statusCode;
            string message;
            object? errors = null;

            switch (ex)
            {
                // ── Custom AppExceptions thrown by services and controllers ────
                case AppException appEx:
                    statusCode = appEx.StatusCode;
                    message = appEx.Message;
                    break;

                // ── FluentValidation pipeline errors ──────────────────────────
                case FluentValidation.ValidationException valEx:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = "One or more validation errors occurred.";
                    errors = valEx.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );
                    break;

                // ── Standard .NET exceptions ──────────────────────────────────
                case KeyNotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    message = ex.Message;
                    break;

                case UnauthorizedAccessException:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    message = "Unauthorized access.";
                    break;

                // ── Catch-all ─────────────────────────────────────────────────
                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    message = "An unexpected error occurred. Please try again later.";
                    break;
            }

            context.Response.StatusCode = statusCode;

            var response = new
            {
                statusCode,
                message,
                errors,
                path = context.Request.Path.ToString(),
                timestamp = DateTime.UtcNow
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
        }
    }
}