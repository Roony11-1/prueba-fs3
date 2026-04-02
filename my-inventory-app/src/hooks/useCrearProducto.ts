import { useMutation, useQueryClient } from "@tanstack/react-query";
import { crearProducto } from "../services/productoService";
import { useAxios } from "./useAxios";
import type { Producto } from "../types";

export const useCrearProducto = () => {
  const queryClient = useQueryClient();
  const api = useAxios();

  return useMutation({
    mutationFn: (nuevoProducto: Producto) => crearProducto(api, nuevoProducto), 
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["productos"] });
    }
  });
};