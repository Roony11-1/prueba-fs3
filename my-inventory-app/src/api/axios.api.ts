import axios from "axios";

export const api = axios.create({
  headers: {
    "Content-Type": "application/json",
  },
});

let keycloakInstance: any = null;

export const setKeycloakInstance = (kc: any) => {
  keycloakInstance = kc;
};

api.interceptors.request.use(
  async (config) => {

    if (keycloakInstance?.token) {
      config.headers.Authorization = `Bearer ${keycloakInstance.token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);