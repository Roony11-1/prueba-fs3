import { useMutation, useQueryClient } from "@tanstack/react-query";
import { crearVenta } from "../services/ventaService";

export const useGenerarVenta = () => 
{
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: crearVenta,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["ventas"] });
    }
  });
};