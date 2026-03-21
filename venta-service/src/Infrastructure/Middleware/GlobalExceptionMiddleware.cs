using System.Net;
using System.Text.Json;
using VentaService.Domain.CustomExceptions;

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

        var (statusCode, message) = ex switch
        {
            InvalidOperationException => ((int)HttpStatusCode.Conflict, ex.Message),

            Polly.CircuitBreaker.BrokenCircuitException =>
                ((int)HttpStatusCode.ServiceUnavailable, "El servicio de inventario está en mantenimiento preventivo (Circuito Abierto)."),

            Polly.Timeout.TimeoutRejectedException =>
                ((int)HttpStatusCode.GatewayTimeout, "El servicio de inventario tardó demasiado en responder."),

            ServiceUnavailableException => ((int)HttpStatusCode.ServiceUnavailable, ex.Message),

            HttpRequestException =>
                ((int)HttpStatusCode.ServiceUnavailable, "No se pudo establecer conexión con el microservicio de inventario."),

            _ => ((int)HttpStatusCode.InternalServerError, "Error interno no controlado.")
        };

        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            error = message,
            status = statusCode,
            timestamp = DateTime.UtcNow
        }));
    }
}