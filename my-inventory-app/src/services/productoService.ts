import axios from "axios";

const API_URL = "http://localhost:5000/api/producto";

const api = axios.create({
  baseURL: API_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

export const obtenerProductos = async () => 
{
  try 
  {
    const res = await api.get("");
    return res.data;
  } 
  catch (err: any) 
  {
    const message = err.response?.data?.message || "Error al obtener productos";
    throw new Error(message);
  }
};

export const crearProducto = async (producto: any) => 
{
  try 
  {
    const res = await api.post("", producto);
    return res.data;
  } 
  catch (err: any) 
  {
    const message = err.response?.data?.message || "Error al crear el producto";
    throw new Error(message);
  }
};