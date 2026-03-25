import { useContext } from "react";
import { KeycloakContext } from "./KeycloakProvider";

export const useKeycloak = () => {
  return useContext(KeycloakContext);
};