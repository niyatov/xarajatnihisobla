using OutlayApi.Exceptions;

namespace OutlayApi.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly string error = "Xatolik sodir bo'ldi, Iltimos keyinroq urinib ko'ring";
    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (NotFoundException e)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            await httpContext.Response.WriteAsJsonAsync(e.Message);
        }
        catch (BadRequestException e)
        {
            httpContext.Response.StatusCode = e.ErrorCode;
            await httpContext.Response.WriteAsJsonAsync(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError($"Message: {e.Message}, StackTrace: {e.StackTrace}", "Internal error");

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(error);
        }
    }
}

public static class ErrorHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandlerMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlerMiddleware>();
    }
}

