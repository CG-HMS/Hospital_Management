using System.Net;
using System.Text.Json;
using FluentValidation;
using Hms.API.Exceptions;

namespace Hms.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
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

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception ex)
        {
            context.Response.ContentType = "application/json";

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
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = "One or more validation errors occurred.";
                    details = string.Join(
                        " | ",
                        valEx.Errors.Select(e => e.ErrorMessage));
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    message = "An unexpected error occurred.";
                    details = ex.Message;
                    break;
            }

            context.Response.StatusCode = statusCode;

            var response = new
            {
                statusCode,
                message,
                details,
                timestamp = DateTime.UtcNow
            };

            var json = JsonSerializer.Serialize(
                response,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            await context.Response.WriteAsync(json);
        }
    }
}