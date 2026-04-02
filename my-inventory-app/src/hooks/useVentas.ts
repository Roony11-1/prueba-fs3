import { useQuery } from "@tanstack/react-query";
import { obtenerVentas } from "../services/ventaService";
import { useAxios } from "./useAxios";

export const useVentas = () => 
{
  const api = useAxios();
  return useQuery({
    queryKey: ["ventas"],
    queryFn: () => obtenerVentas(api)
    
  });
};