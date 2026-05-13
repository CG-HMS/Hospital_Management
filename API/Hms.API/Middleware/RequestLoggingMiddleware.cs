using System.Diagnostics;

namespace Hms.API.Middleware;

/// <summary>
/// Middleware to log incoming HTTP requests and responses
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestId = Guid.NewGuid().ToString();
        
        context.Items["RequestId"] = requestId;

        _logger.LogInformation(
            "Incoming Request: {RequestId} {Method} {Path} {QueryString}",
            requestId,
            context.Request.Method,
            context.Request.Path,
            context.Request.QueryString
        );

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            
            _logger.LogInformation(
                "Completed Request: {RequestId} {Method} {Path} {StatusCode} - {ElapsedMs}ms",
                requestId,
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds
            );
        }
    }
}