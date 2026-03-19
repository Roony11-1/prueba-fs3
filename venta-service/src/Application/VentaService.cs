using VentaService.Domain;
using VentaService.Infrastructure.Messaging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace VentaService.Application;

public class VentaService(IVentaRepository ventaRepository) : IVentaService
{
    private readonly IVentaRepository _ventaRepository = ventaRepository;

    private readonly AsyncRetryPolicy _retryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(250));

    public async Task<Venta> CrearVenta(Venta venta)
    {
        if (venta.Detalles == null || venta.Detalles.Count == 0)
            throw new Exception("La venta debe tener al menos un item");

        venta.Fecha = DateTimeOffset.UtcNow;

        try
        {
            var ventaSaved = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _ventaRepository.AddAsync(venta);
            });

            var producer = new KafkaProducer("kafka:9092");
            await producer.EnviarMensajeAsync("venta-realizada", ventaSaved.ToString());

            return ventaSaved;
        }
        catch (Exception ex)
        {
            // fallback
            return await FallbackCrearVenta(venta, ex);
        }
    }

    private async Task<Venta> FallbackCrearVenta(Venta venta, Exception ex)
    {
        throw new Exception("No se pudo crear la venta");
    }

    public async Task<List<Venta>> GetAll()
    {
        try
        {
            var ventas = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _ventaRepository.GetAllAsync();
            });

            return ventas;
        }
        catch (Exception ex)
        {
            // fallback
            return await FallbackGetAll(ex);
        }
    }

    public async Task<List<Venta>> FallbackGetAll(Exception ex)
    {
        throw new Exception("No se pudieron obtener las ventas");
    }

    public async Task<Venta?> GetById(int id)
    {
        return await _ventaRepository.GetByIdAsync(id);
    }
}