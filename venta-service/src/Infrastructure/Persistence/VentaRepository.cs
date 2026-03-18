using Polly;
using Microsoft.EntityFrameworkCore;
using VentaService.Domain;
using Microsoft.Data.SqlClient;

namespace VentaService.Infrastructure.Persistence
{
    public class VentaRepository : IVentaRepository
    {
        private readonly AppDbContext _context;
        private readonly IAsyncPolicy _retryPolicy;

        public VentaRepository(AppDbContext context)
        {
            _context = context;

            // Crear una política de reintento asincrónica
            _retryPolicy = Policy.Handle<Exception>()  // Maneja las excepciones de tipo SqlException (comunes en las bases de datos)
                                 .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));  // Retrasos exponenciales: 1, 2, 4 segundos
        }

        public async Task<Venta> AddAsync(Venta venta)
        {
            if (DateTime.Now.Second % 2 == 0) // Alternar el fallo para que ocurra en ocasiones
            {
                throw new Exception("Simulando una falla de base de datos");
            }

            // Normalmente, esta lógica agregaría la venta
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
            // Aplicar la política de reintentos a la operación de base de datos
            return _retryPolicy.ExecuteAsync(async () =>
            {
                return await _context.Ventas
                    .Include(v => v.Detalles)
                    .ToListAsync();
            });
        }

        public async Task<Venta?> GetByIdAsync(int id)
        {
            // Aplicar la política de reintentos a la operación de base de datos
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _context.Ventas
                    .Include(v => v.Detalles)
                    .FirstOrDefaultAsync(v => v.Id == id);
            });
        }
    }
}