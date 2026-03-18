const API_URL = "http://localhost:5000/api/producto";

export const obtenerProductos = async () => {
  const res = await fetch(API_URL);
  return res.json();
};

export const crearProducto = async (producto: any) => {
  const res = await fetch(API_URL, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(producto),
  });

  if (!res.ok) throw new Error("Error al crear producto");

  return res.json();
};