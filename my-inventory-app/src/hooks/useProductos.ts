// src/hooks/useProductos.ts
import { useQuery } from "@tanstack/react-query";
import { obtenerProductos } from "../services/productoService";
import { useKeycloak } from "../auth/useKeyCloak";

export const useProductos = () => {
  const { authenticated } = useKeycloak();

  return useQuery({
    queryKey: ["productos"],
    queryFn: obtenerProductos,
    enabled: authenticated
  });
};