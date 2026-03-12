namespace InventoryService.Models;

public class Venta
{
    public int Id { get; set; }
    public IList<VentaDetalle> Detalles { get; set; } = [];
    public DateOnly Fecha { get; set; } = DateOnly.FromDateTime(DateTime.Now);
}