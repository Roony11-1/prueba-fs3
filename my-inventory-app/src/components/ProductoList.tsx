import type { Producto } from "../types";

interface Props {
  productos: Producto[];
}

export default function ProductoList({ productos }: Props) {
  return (
    <>
      <h2>Productos</h2>
      <ul>
        {productos.map((p) => (
          <li key={p.id}>
            {p.nombre} - ${p.precio} - Stock: {p.stock}
          </li>
        ))}
      </ul>
    </>
  );
}