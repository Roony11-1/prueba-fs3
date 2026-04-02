import type { AxiosInstance } from "axios";
import type { Producto } from "../types";

const API_URL = "http://localhost:5000/api/producto";

export const obtenerProductos = async (api: AxiosInstance) => 
{
  const res = await api.get(API_URL);
  return res.data;
};

export const crearProducto = async (api: AxiosInstance, producto: Producto) => 
{
  const res = await api.post(API_URL, producto);
  return res.data;
};