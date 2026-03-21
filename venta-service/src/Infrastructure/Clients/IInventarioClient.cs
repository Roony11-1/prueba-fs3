namespace VentaService.Infrastructure.Clients;

public interface IInventarioClient
{
    Task<bool> ValidarStockAsync(int productoId, int cantidad);
}