using System.Text;

namespace VentaService.Domain;

public class Venta
{
    public int Id { get; set; }

    public List<VentaDetalle> Detalles { get; set; } = [];

    public DateTimeOffset Fecha { get; set; }
}