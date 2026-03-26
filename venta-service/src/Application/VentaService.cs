using VentaService.Domain;
using VentaService.Application.Interfaces;
using System.Text.Json;
using VentaService.Infrastructure.Common;
using System.Threading.Channels;
using VentaService.Infrastructure.Clients;

namespace VentaService.Application;

public class VentaService(
    IVentaRepository ventaRepository,
    IUnitOfWork unitOfWork,
    Channel<bool> outBoxChannel,
    IInventarioClient inventarioClient) : IVentaService
{
    public async Task<Venta> CrearVenta(Venta venta)
    {
        ValidarReglasDeVenta(venta);

        foreach (var detalle in venta.Detalles)
        {
            var tieneStock = await inventarioClient.ValidarStockAsync(detalle.ProductoId, detalle.Cantidad);

            if (!tieneStock)
            {
                throw new InvalidOperationException($"No hay stock para el producto {detalle.ProductoId}.");
            }
        }

        try
        {
            await GuardarVentaYOutbox(venta);
            return venta;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al procesar la venta: " + ex.Message);
        }
    }

    private void ValidarReglasDeVenta(Venta venta)
    {
        if (venta.Detalles == null || venta.Detalles.Count == 0)
            throw new Exception("La venta debe tener al menos un item");
    }

    private async Task GuardarVentaYOutbox(Venta venta)
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
    }

    public async Task<List<Venta>> GetAllByUsuarioId(string usuarioId)
    {
        return await ventaRepository.GetAllByUserId(usuarioId);
    }

    public async Task<Venta?> GetById(int id)
    {
        return await ventaRepository.GetByIdAsync(id);
    }
}