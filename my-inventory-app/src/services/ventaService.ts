import { api } from "../api/axios.api";
import type { Venta } from "../types";

const API_URL = "http://localhost:5152/api/venta";

export const obtenerVentas = async () => {
  try {
    const res = await api.get(API_URL);
    return res.data;
  } catch (err: any) {
    const message =
      err.response?.data?.error || "Error al obtener ventas";
    throw new Error(message);
  }
};

export const crearVenta = async (venta: Venta) => {
  try {
    const res = await api.post(API_URL, venta);
    return res.data;
  } catch (err: any) {
    const message =
      err.response?.data?.error || "Error al realizar la venta";
    throw new Error(message);
  }
};