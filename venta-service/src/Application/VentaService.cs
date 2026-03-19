using VentaService.Domain;
using VentaService.Infrastructure.Messaging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using VentaService.Infrastructure.Persistence;

namespace VentaService.Application;

public class VentaService(IVentaRepository ventaRepository, AppDbContext context) : IVentaService
{
    private readonly IVentaRepository _ventaRepository = ventaRepository;
    private readonly AppDbContext _context = context;

    private readonly AsyncRetryPolicy _retryDbPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(250));

    private readonly AsyncRetryPolicy _retryKafkaPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(150));

    private readonly AsyncCircuitBreakerPolicy _circuitKafka = Policy
        .Handle<Exception>()
        .AdvancedCircuitBreakerAsync(
            failureThreshold: 0.5,
            samplingDuration: TimeSpan.FromSeconds(10),
            minimumThroughput: 6,
            durationOfBreak: TimeSpan.FromSeconds(20)
        );

    public async Task<Venta> CrearVenta(Venta venta)
    {
        if (venta.Detalles == null || venta.Detalles.Count == 0)
            throw new Exception("La venta debe tener al menos un item");

        venta.Fecha = DateTimeOffset.UtcNow;

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var ventaSaved = await _retryDbPolicy.ExecuteAsync(() =>
                _ventaRepository.AddAsync(venta)
            );

            var producer = new KafkaProducer("kafka:9092");

            await _circuitKafka.ExecuteAsync(async () =>
            {
                await _retryKafkaPolicy.ExecuteAsync(async () =>
                {
                    await producer.EnviarMensajeAsync("venta-realizada", ventaSaved.ToString());
                });
            });

            await transaction.CommitAsync();

            return ventaSaved;
        }
        catch (BrokenCircuitException)
        {
            await transaction.RollbackAsync();
            return await FallbackKafka(new Exception("Circuito abierto (Kafka caído)"));
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            if (EsErrorKafka(ex))
                return await FallbackKafka(ex);

            return await FallbackCrearVenta(ex);
        }
    }

    private bool EsErrorKafka(Exception ex)
    {
        return ex is BrokenCircuitException ||
               ex.Message.Contains("kafka", StringComparison.OrdinalIgnoreCase) ||
               ex.Message.Contains("broker", StringComparison.OrdinalIgnoreCase);
    }

    private Task<Venta> FallbackKafka(Exception ex)
    {
        throw new Exception("Servicio de Kafka caído!");
    }

    private Task<Venta> FallbackCrearVenta(Exception ex)
    {
        throw new Exception("No se pudo crear la venta");
    }

    public async Task<List<Venta>> GetAll()
    {
        try
        {
            return await _retryDbPolicy.ExecuteAsync(() =>
                _ventaRepository.GetAllAsync()
            );
        }
        catch (Exception ex)
        {
            return await FallbackGetAll(ex);
        }
    }

    private Task<List<Venta>> FallbackGetAll(Exception ex)
    {
        throw new Exception("No se pudieron obtener las ventas");
    }

    public async Task<Venta?> GetById(int id)
    {
        return await _ventaRepository.GetByIdAsync(id);
    }
}