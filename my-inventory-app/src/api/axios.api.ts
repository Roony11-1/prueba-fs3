import { useAuth0 } from "@auth0/auth0-react";
import axios from "axios";

export const api = axios.create({
  headers: {
    "Content-Type": "application/json",
  },
});

const { getAccessTokenSilently } = useAuth0();


api.interceptors.request.use(
  async (config) => 
  {
      const token = await getAccessTokenSilently();
      
      config.headers.Authorization = `Bearer ${token}`;
    return config;
  },
  (error) => Promise.reject(error)
);