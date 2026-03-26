using System.Text;

namespace VentaService.Domain;

public class Venta
{
    public int Id { get; set; }
    
    public string UsuarioId { get; set; } = string.Empty;

    public List<VentaDetalle> Detalles { get; set; } = [];

    public DateTimeOffset Fecha { get; set; }
}