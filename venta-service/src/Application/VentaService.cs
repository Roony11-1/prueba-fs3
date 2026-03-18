using VentaService.Domain;
using VentaService.Infrastructure.Messaging;

namespace VentaService.Application;

public class VentaService(IVentaRepository ventaRepository) : IVentaService
{
    private readonly IVentaRepository _ventaRepository = ventaRepository;

    public async Task<Venta> CrearVenta(Venta venta)
    {
        if (venta.Detalles == null || venta.Detalles.Count == 0)
            throw new Exception("La venta debe tener al menos un item");

        venta.Fecha = DateTimeOffset.UtcNow;

        var ventaSaved = await _ventaRepository.AddAsync(venta);

        var producer = new KafkaProducer("localhost:9092");

        await producer.EnviarMensajeAsync("venta-realizada", ventaSaved.ToString());

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