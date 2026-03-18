using System.Text;
using System.Text.Json.Serialization;

namespace VentaService.Domain;

public class VentaDetalle
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }

    public int? VentaId { get; set; }   // FK

    [JsonIgnore]
    public Venta? Venta { get; set; }   // navegación

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append(this.ProductoId).Append('-').Append(this.Cantidad);

        return sb.ToString();
    }
}