import { useState } from "react";

interface Props {
  onCrear: (producto: any) => void;
}

export default function ProductoForm({ onCrear }: Props) {
  const [nombre, setNombre] = useState("");
  const [precio, setPrecio] = useState("");
  const [stock, setStock] = useState("");

  const handleSubmit = () => {
    if (!nombre || !precio || !stock) {
      alert("Todos los campos son obligatorios");
      return;
    }

    onCrear({
      nombre,
      precio: parseFloat(precio),
      stock: parseInt(stock),
    });

    setNombre("");
    setPrecio("");
    setStock("");
  };

  return (
    <>
      <h2>Crear Producto</h2>
      <input placeholder="Nombre" value={nombre} onChange={(e) => setNombre(e.target.value)} />
      <input type="number" placeholder="Precio" value={precio} onChange={(e) => setPrecio(e.target.value)} />
      <input type="number" placeholder="Stock" value={stock} onChange={(e) => setStock(e.target.value)} />
      <button onClick={handleSubmit}>Guardar</button>
    </>
  );
}