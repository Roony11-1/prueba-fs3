import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./App.tsx";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { Auth0Provider } from "@auth0/auth0-react";

const queryClient = new QueryClient();

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <Auth0Provider
      domain="dev-pl1b5b2qt0uh7j45.us.auth0.com"
      clientId="UO7NHVMUn5X7KAltVX071t52C6FgHPb0"
      authorizationParams={{ redirect_uri: window.location.origin, audience: "https://my-api" }} >
      <QueryClientProvider client={queryClient}>
        <App />
      </QueryClientProvider>
    </Auth0Provider>
  </StrictMode>,
);
