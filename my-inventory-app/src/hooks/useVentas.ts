import { useQuery } from "@tanstack/react-query";
import { obtenerVentas } from "../services/ventaService";
import { useKeycloak } from "../auth/useKeyCloak";

export const useVentas = () => {
  const { authenticated } = useKeycloak();

  return useQuery({
    queryKey: ["ventas"],
    queryFn: obtenerVentas,
    enabled: authenticated
  });
};