using VentaService.Domain;
using VentaService.Infrastructure.Messaging;
using Polly;
using Polly.CircuitBreaker;

namespace VentaService.Application;

public class VentaService(IVentaRepository ventaRepository) : IVentaService
{
    private readonly IVentaRepository _ventaRepository = ventaRepository;
    // Circuit Breaker
    private readonly AsyncCircuitBreakerPolicy _circuitBreaker = Policy
        .Handle<Exception>()
        .CircuitBreakerAsync(
            exceptionsAllowedBeforeBreaking: 3,
            durationOfBreak: TimeSpan.FromSeconds(10)
        );

    public async Task<Venta> CrearVenta(Venta venta)
    {
        if (venta.Detalles == null || venta.Detalles.Count == 0)
            throw new Exception("La venta debe tener al menos un item");

        venta.Fecha = DateTimeOffset.UtcNow;

        var ventaSaved = await _ventaRepository.AddAsync(venta);

        var producer = new KafkaProducer("kafka:9092");

        // AQUÍ se aplica el Circuit Breaker
        await _circuitBreaker.ExecuteAsync(async () =>
        {
            await producer.EnviarMensajeAsync("venta-realizada", ventaSaved.ToString());
        });

        return ventaSaved;
    }

    public async Task<List<Venta>> GetAll()
    {
        return await _ventaRepository.GetAllAsync();
    }

    public async Task<Venta?> GetById(int id)
    {
        return await _ventaRepository.GetByIdAsync(id);
    }
}