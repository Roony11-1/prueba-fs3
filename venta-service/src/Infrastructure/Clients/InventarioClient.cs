using System.Net;
using Polly.CircuitBreaker;
using Polly.Timeout;
using VentaService.Domain.CustomExceptions;

namespace VentaService.Infrastructure.Clients;

public class InventarioClient(
    IHttpClientFactory httpClientFactory,
    ILogger<InventarioClient> logger) : IInventarioClient
{
    private readonly string _inventoryEndPoint = "api/producto";
    public async Task<bool> ValidarStockAsync(int productoId, int cantidad)
    {
        var client = httpClientFactory.CreateClient("InventarioClient");
        var validarUrl = $"api/producto/{productoId}/validar-stock?cantidad={cantidad}";

        try
        {
            var response = await client.GetAsync(validarUrl);

            if (response.StatusCode == HttpStatusCode.BadRequest) return false;

            response.EnsureSuccessStatusCode(); // Lanza error si es 5xx

            return await response.Content.ReadFromJsonAsync<bool>();
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "El servicio de Inventario no responde.");
            // Lanzamos nuestra excepción específica
            throw new ServiceUnavailableException("El servicio de Inventario está temporalmente fuera de línea.");
        }
    }
}