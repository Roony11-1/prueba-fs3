using InventoryService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Data;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
    {
        
    }

    public DbSet<Producto> Productos { get; set; }
    public DbSet<Venta> Ventas { get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Venta>()
            .HasMany(venta => venta.Detalles)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}