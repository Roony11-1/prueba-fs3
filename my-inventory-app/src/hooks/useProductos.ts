// src/hooks/useProductos.ts
import { useQuery } from "@tanstack/react-query";
import { obtenerProductos } from "../services/productoService";
import { useAxios } from "./useAxios";

export const useProductos = () => 
{
  const api = useAxios();
  return useQuery({
    queryKey: ["productos"],
    queryFn: () => obtenerProductos(api)
  });
};