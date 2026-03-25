import { useMutation, useQueryClient } from "@tanstack/react-query";
import { crearProducto } from "../services/productoService";

export const useCrearProducto = () => 
{
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: crearProducto,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["productos"] });
    }
  });
};