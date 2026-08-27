using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Premium_ServiceAPI.Middleware;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IHostEnvironment environment)
{
    public async Task Invoke(HttpContext context)
    {
        try { await next(context); }
        catch (ValidationException exception)
        {
            logger.LogWarning(exception, "Validation error while processing request");
            await WriteErrorAsync(context, StatusCodes.Status400BadRequest, "Validation error", exception.Message);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled exception while processing request");
            await WriteErrorAsync(context, StatusCodes.Status500InternalServerError, "Internal server error", environment.IsDevelopment() ? exception.Message : "An unexpected error occurred.");
        }
    }

    private static Task WriteErrorAsync(HttpContext context, int statusCode, string title, string detail)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync(JsonSerializer.Serialize(new { title, status = statusCode, detail, traceId = context.TraceIdentifier }));
    }
}