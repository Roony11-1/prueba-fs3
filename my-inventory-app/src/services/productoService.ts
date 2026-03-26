import { api } from "../api/axios.api";
import type { Producto } from "../types";

const API_URL = "http://localhost:5000/api/producto";

export const obtenerProductos = async () => {
  try {
    const res = await api.get(API_URL);
    return res.data;
  } catch (err: any) {
    const message =
      err.response?.data?.message || "Error al obtener productos";
    throw new Error(message);
  }
};

export const crearProducto = async (producto: Producto) => {
  try {
    const res = await api.post(API_URL, producto);
    return res.data;
  } catch (err: any) {
    const message =
      err.response?.data?.message || "Error al crear el producto";
    throw new Error(message);
  }
};