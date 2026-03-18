using Microsoft.EntityFrameworkCore;
using VentaService.Domain;

namespace VentaService.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Venta> Ventas { get; set; }
    public DbSet<VentaDetalle> VentasDetalles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Venta>()
            .HasMany(v => v.Detalles)
            .WithOne(d => d.Venta)
            .HasForeignKey(d => d.VentaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}