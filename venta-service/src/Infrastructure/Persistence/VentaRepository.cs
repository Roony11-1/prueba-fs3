using Polly;
using Microsoft.EntityFrameworkCore;
using VentaService.Domain;
using Microsoft.Data.SqlClient;

namespace VentaService.Infrastructure.Persistence;

public class VentaRepository : IVentaRepository
{
    private readonly AppDbContext _context;

    public VentaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Venta> AddAsync(Venta venta)
    {
        // Normalmente, esta lógica agregaría la venta
        foreach (var detalle in venta.Detalles)
        {
            detalle.Venta = venta;
        }

        _context.Ventas.Add(venta);
        await _context.SaveChangesAsync();
        return venta;
    }

    public async Task<List<Venta>> GetAllByUserId(string userId)
    {
        return await _context.Ventas
            .Include(v => v.Detalles)
            .Where(v => v.UsuarioId == userId)
            .ToListAsync();
    }

    public async Task<Venta?> GetByIdAsync(int id)
    {
        return await _context.Ventas
            .Include(v => v.Detalles)
            .FirstOrDefaultAsync(v => v.Id == id);
    }
}