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

    private readonly AsyncRetryPolicy _retryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(250));

    public async Task<Venta> CrearVenta(Venta venta)
    {
        if (venta.Detalles == null || venta.Detalles.Count == 0)
            throw new Exception("La venta debe tener al menos un item");

        venta.Fecha = DateTimeOffset.UtcNow;

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var ventaSaved = await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _ventaRepository.AddAsync(venta);
            });

            var producer = new KafkaProducer("kafka:9092");

            try
            {
                await producer.EnviarMensajeAsync("venta-realizada", ventaSaved.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("KAFKA_ERROR", ex);
            }

            await transaction.CommitAsync();

            return ventaSaved;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            if (ex.Message == "KAFKA_ERROR")
                return await FallbackKafka(ex.InnerException!);

            return await FallbackCrearVenta(ex);
        }
    }

    private async Task<Venta> FallbackKafka(Exception ex)
    {
        throw new Exception("Servicio de Kafka caido!");
    }

    private async Task<Venta> FallbackCrearVenta(Exception ex)
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