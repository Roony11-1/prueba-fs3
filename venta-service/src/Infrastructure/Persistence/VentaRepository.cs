using Microsoft.EntityFrameworkCore;
using VentaService.Domain;

namespace VentaService.Infrastructure.Persistence;

public class VentaRepository(AppDbContext context) : IVentaRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Venta> AddAsync(Venta venta)
    {
        foreach (var detalle in venta.Detalles)
        {
            detalle.Venta = venta;
        }
        
        _context.Ventas.Add(venta);
        await _context.SaveChangesAsync();
        return venta;
    }

    public Task<List<Venta>> GetAllAsync()
    {
        return _context.Ventas
            .Include(v => v.Detalles)
            .ToListAsync();
    }

    public async Task<Venta?> GetByIdAsync(int id)
    {
        return await _context.Ventas
            .Include(v => v.Detalles)
            .FirstOrDefaultAsync(v => v.Id == id);
    }
}
