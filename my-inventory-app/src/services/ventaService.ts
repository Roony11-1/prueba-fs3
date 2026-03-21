import axios from "axios";

const API_URL = "http://localhost:5152/api/venta";

const api = axios.create({
  baseURL: API_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

export const obtenerVentas = async () => 
{
  try 
  {
    const res = await api.get("/");
    return res.data;
  } 
  catch (err: any) 
  {
    const message = err.response?.data?.error || "Error al obtener ventas";
    throw new Error(message);
  }
};

export const crearVenta = async (venta: any) => 
{
  try 
  {
    const res = await api.post("/", venta);
    return res.data;
  } 
  catch (err: any) 
  {
    const message = err.response?.data?.error || "Error al realizar la venta";
    
    console.error(`Status: ${err.response?.status}`);
    
    throw new Error(message);
  }
};