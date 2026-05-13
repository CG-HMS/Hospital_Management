using System.Net;
using System.Text.Json;
using Hms.API.Exceptions;
using FluentValidationException = FluentValidation.ValidationException;

namespace Hms.API.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger)
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
            _logger.LogError(ex,
                "An unhandled exception occurred: {Message}",
                ex.Message);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message, errors) = exception switch
        {
            NotFoundException ex => (
                HttpStatusCode.NotFound,
                ex.Message,
                null as object
            ),

            BadRequestException ex => (
                HttpStatusCode.BadRequest,
                ex.Message,
                null as object
            ),

            ValidationException ex => (
                HttpStatusCode.BadRequest,
                ex.Message,
                null as object
            ),

            FluentValidationException ex => (
                HttpStatusCode.BadRequest,
                "One or more validation errors occurred.",
                ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    ) as object
            ),

            KeyNotFoundException ex => (
                HttpStatusCode.NotFound,
                ex.Message,
                null as object
            ),

            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                "Unauthorized access.",
                null as object
            ),

            _ => (
                HttpStatusCode.InternalServerError,
                "An internal server error occurred.",
                null as object
            )
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new ErrorResponse
        {
            StatusCode = (int)statusCode,
            Message = message,
            Errors = errors,
            Timestamp = DateTime.UtcNow,
            Path = context.Request.Path.ToString()
        };

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var jsonResponse = JsonSerializer.Serialize(response, options);

        await context.Response.WriteAsync(jsonResponse);
    }
}

public class ErrorResponse
{
    public int StatusCode { get; set; }

    public string Message { get; set; } = string.Empty;

    public object? Errors { get; set; }

    public DateTime Timestamp { get; set; }

    public string Path { get; set; } = string.Empty;
}