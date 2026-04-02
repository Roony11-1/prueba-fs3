import axios from "axios";

export const api = axios.create({
  headers: {
    "Content-Type": "application/json",
  },
});

const token: (string | undefined) = undefined;

api.interceptors.request.use(
  async (config) => 
  {
      
      config.headers.Authorization = `Bearer ${token}`;
    return config;
  },
  (error) => Promise.reject(error)
);