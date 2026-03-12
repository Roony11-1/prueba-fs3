namespace InventoryService.Models;

public class VentaRealizadaEvent
{
    public int VentaId { get; set; }
    public List<VentaDetalleEvent> Detalles { get; set; } = new();
}

public class VentaDetalleEvent
{
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
}