using System.Net;
using System.Text.Json;

namespace VentaService.Infrastructure.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private static async Task HandleException(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        var statusCode = 503;

        var response = new
        {
            message = ex.Message,
            status = statusCode
        };

        context.Response.StatusCode = statusCode;

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}