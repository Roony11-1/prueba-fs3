import type { Venta, Producto } from "../types";

interface Props {
  ventas: Venta[];
  productos: Producto[];
}

export default function VentasList({ ventas, productos }: Props) {
  return (
    <>
      <h1>Ventas realizadas</h1>
      <div style={{ display: "flex", flexDirection: "row", flexWrap: "wrap" }}>
        {ventas.map((v) => (
          <div key={v.id} style={{ marginBottom: "20px" }}>
            <h3>Venta #{v.id}</h3>

            {v.detalles.map((d) => {
              const producto = productos.find((p) => p.id === d.productoId);

              return (
                <div key={d.productoId}>
                  <span>
                    {producto ? producto.nombre : `Producto ID ${d.productoId}`}
                  </span>
                  <span> - Cantidad: {d.cantidad}</span>
                </div>
              );
            })}

            <div>
              Fecha:{" "}
              {v.fecha
                ? new Date(v.fecha).toLocaleDateString("es-CL")
                : "Sin fecha"}
            </div>
          </div>
        ))}
      </div>
    </>
  );
}
