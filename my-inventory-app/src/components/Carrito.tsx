import type { Producto, VentaDetalle } from "../types";

interface Props {
  productos: Producto[];
  carrito: VentaDetalle[];
  setCarrito: React.Dispatch<React.SetStateAction<VentaDetalle[]>>;
  onVender: () => void;
}

export default function Carrito({ productos, carrito, setCarrito, onVender }: Props) {
  return (
    <>
      <h1>Zona de ventas</h1>

      {productos.map((p) => {
        const carritoItem = carrito.find((item) => item.productoId === p.id);
        const estaEnCarrito = !!carritoItem;

        return (
          <div key={p.id} style={{ border: "1px solid #ccc", padding: "10px", marginBottom: "10px" }}>
            <span>
              {p.nombre} - ${p.precio} - Stock: {p.stock}
            </span>

            <button
              onClick={() => {
                if (estaEnCarrito) {
                  setCarrito((prev) => prev.filter((item) => item.productoId !== p.id));
                } else {
                  setCarrito((prev) => [...prev, { productoId: p.id, cantidad: 1 }]);
                }
              }}
            >
              {estaEnCarrito ? "Quitar" : "Añadir"}
            </button>

            {estaEnCarrito && (
              <input
                type="number"
                min={1}
                max={p.stock}
                value={carritoItem.cantidad}
                onChange={(e) => {
                  const cantidad = Math.min(Math.max(1, Number(e.target.value)), p.stock);

                  setCarrito((prev) =>
                    prev.map((item) =>
                      item.productoId === p.id ? { ...item, cantidad } : item
                    )
                  );
                }}
              />
            )}
          </div>
        );
      })}

      <button onClick={onVender}>Vender</button>
    </>
  );
}