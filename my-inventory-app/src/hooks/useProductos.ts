// src/hooks/useProductos.ts
import { useQuery } from "@tanstack/react-query";
import { obtenerProductos } from "../services/productoService";

export const useProductos = () => {
  return useQuery({
    queryKey: ["productos"],
    queryFn: obtenerProductos
  });
};