import type { AxiosInstance } from "axios";
import type { Venta } from "../types";

const API_URL = "http://localhost:5152/api/venta";

export const obtenerVentas = async (api: AxiosInstance): Promise<Venta[]> => 
{
  const res = await api.get(API_URL);
  return res.data;
};

export const crearVenta = async (api: AxiosInstance, venta: Venta) => 
{
  const res = await api.post(API_URL, venta);
  return res.data;
};