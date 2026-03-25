import { createContext, useEffect, useState } from "react";
import Keycloak from "keycloak-js";
import { setKeycloakInstance } from "../api/axios.api";

const keycloakOptions = {
  url: "http://localhost:8080",
  realm: "reino-neytan",
  clientId: "ricardito",
};

export const KeycloakContext = createContext<any>(null);

export const KeycloakProvider = ({ children }: any) => {
  const [keycloak, setKeycloak] = useState<Keycloak | null>(null);
  const [authenticated, setAuthenticated] = useState(false);

  useEffect(() => {
    const init = async () => {
      const kc = new Keycloak(keycloakOptions);

      try {
        const auth = await kc.init({
          onLoad: "login-required",
        });

        if (auth)
          setKeycloakInstance(kc);
        
        setKeycloak(kc);
        setAuthenticated(auth);
      } catch (err) {
        console.error("Error Keycloak", err);
      }
    };

    init();
  }, []);

  return (
    <KeycloakContext.Provider value={{ keycloak, authenticated }}>
      {children}
    </KeycloakContext.Provider>
  );
};
