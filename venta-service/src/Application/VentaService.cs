using VentaService.Domain;
using VentaService.Application.Interfaces;
using System.Text.Json;
using VentaService.Infrastructure.Common;
using System.Threading.Channels;

namespace VentaService.Application;

// Nota: Usamos primary constructors como tenías
public class VentaService(
    IVentaRepository ventaRepository,
    IUnitOfWork unitOfWork,
    Channel<bool> outBoxChannel) : IVentaService
{
    public async Task<Venta> CrearVenta(Venta venta)
    {
        // 1. Validaciones básicas
        if (venta.Detalles == null || venta.Detalles.Count == 0)
            throw new Exception("La venta debe tener al menos un item");

        venta.Fecha = DateTimeOffset.UtcNow;

        try
        {
            await ventaRepository.AddAsync(venta);

            var outboxMessage = new OutboxMessage
            {
                Type = "venta-realizada",
                Payload = JsonSerializer.Serialize(venta, JsonDefaults.CamelCaseOptions)
            };

            await unitOfWork.AddOutboxMessageAsync(outboxMessage);

            await unitOfWork.CommitAsync();

            await outBoxChannel.Writer.WriteAsync(true);

            return venta;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al procesar la venta: " + ex.Message);
        }
    }

    public async Task<List<Venta>> GetAll()
    {
        return await ventaRepository.GetAllAsync();
    }

    public async Task<Venta?> GetById(int id)
    {
        return await ventaRepository.GetByIdAsync(id);
    }
}