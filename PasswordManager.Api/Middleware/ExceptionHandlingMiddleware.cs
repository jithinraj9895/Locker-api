using System.Net;
using System.Text.Json;
using Serilog;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            LogError(context, ex);   // 👈 Log the exception
            await HandleExceptionAsync(context, ex);
        }
    }

    private void LogError(HttpContext context, Exception ex)
    {
        var requestPath = context.Request.Path;
        var method = context.Request.Method;

        Log.Error(ex, "Unhandled exception for {Method} {Path}", method, requestPath);
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var statusCode = ex switch
        {
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            ArgumentException => (int)HttpStatusCode.BadRequest,
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var problem = new
        {
            status = statusCode,
            title = ex.Message,
            traceId = context.TraceIdentifier,
            timestamp = DateTime.UtcNow
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        return context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
