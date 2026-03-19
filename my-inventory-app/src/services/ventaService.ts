const API_URL = "http://localhost:5152/api/venta";

export const obtenerVentas = async () => {
  const res = await fetch(API_URL);

  if (!res.ok) {
    let message = "Error al obtener ventas (no api)";

    const errorData = await res.json();
    message = errorData.message || message;

    throw new Error(message);
  }

  return res.json();
};

export const crearVenta = async (venta: any) => {
  const res = await fetch(API_URL, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(venta),
  });

  if (!res.ok) {
    let message = "Error al realizar la venta (no api)";

    const errorData = await res.json();
    message = errorData.message || message;

    throw new Error(message);
  }

  return res.json();
};
