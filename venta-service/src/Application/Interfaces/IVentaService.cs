using VentaService.Domain;

namespace VentaService.Application;

public interface IVentaService
{
    Task<Venta> CrearVenta(Venta venta);
    Task<Venta?> GetById(int id);
    Task<List<Venta>> GetAllByUsuarioId(string usuarioId);
}