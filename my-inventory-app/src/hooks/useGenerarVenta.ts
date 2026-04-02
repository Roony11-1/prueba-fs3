import { useMutation, useQueryClient } from "@tanstack/react-query";
import { crearVenta } from "../services/ventaService";
import { useAxios } from "./useAxios";
import type { Venta } from "../types";

export const useGenerarVenta = () => 
{
  const queryClient = useQueryClient();
  const api = useAxios();

  return useMutation({
    mutationFn: (ventaNueva: Venta) => crearVenta(api, ventaNueva),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["ventas"] });
    }
  });
};