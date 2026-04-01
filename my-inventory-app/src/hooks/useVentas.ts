import { useQuery } from "@tanstack/react-query";
import { obtenerVentas } from "../services/ventaService";

export const useVentas = () => {
  return useQuery({
    queryKey: ["ventas"],
    queryFn: obtenerVentas
  });
};