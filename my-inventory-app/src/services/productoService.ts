const API_URL = "http://localhost:5000/api/producto";

export const obtenerProductos = async () => {
  const res = await fetch(API_URL);

  if (!res.ok) {
    let message = "Error al obtener productos (no lo traje del api)";

    const errorData = await res.json(); // ahora debería funcionar
    message = errorData.message || message;

    throw new Error(message);
  }

  return res.json();
};

export const crearProducto = async (producto: any) => {
  const res = await fetch(API_URL, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(producto),
  });

  if (!res.ok) {
    let message = "Error al crear producto (no viene del api)";

    const errorData = await res.json();
    message = errorData.message || message;

    throw new Error(message);
  }

  return res.json();
};
