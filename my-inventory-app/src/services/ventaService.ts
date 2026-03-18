const API_URL = "http://localhost:5152/api/venta";

export const obtenerVentas = async () => {
  const res = await fetch(API_URL);
  return res.json();
};

export const crearVenta = async (venta: any) => {
  const res = await fetch(API_URL, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(venta),
  });

  if (!res.ok) throw new Error("Error al realizar la venta");

  return res.text();
};