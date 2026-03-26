import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./App.tsx";
import { KeycloakProvider } from "./auth/KeycloakProvider.tsx";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

const queryClient = new QueryClient();

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <KeycloakProvider>
        <App />
      </KeycloakProvider>
    </QueryClientProvider>
  </StrictMode>,
);
