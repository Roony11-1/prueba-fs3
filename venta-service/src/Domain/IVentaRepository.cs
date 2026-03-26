namespace VentaService.Domain;

public interface IVentaRepository
{
    Task<Venta> AddAsync(Venta venta);
    Task<Venta?> GetByIdAsync(int id);
    Task<List<Venta>> GetAllByUserId(string userId);
}