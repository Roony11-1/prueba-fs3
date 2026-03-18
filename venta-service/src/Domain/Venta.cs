using System.Text;

namespace VentaService.Domain;

public class Venta
{
    public int Id { get; set; }

    public List<VentaDetalle> Detalles { get; set; } = [];

    public DateTimeOffset Fecha { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append(this.Id).Append(':');

        for (var i = 0; i < Detalles.Count; i++)
        {
            if (i > 0)
                sb.Append(';');

            sb.Append(Detalles[i].ToString());    
        }
        return sb.ToString();
    }
}