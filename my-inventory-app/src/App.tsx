import { useState } from "react";
import "./App.css";

import ProductoForm from "./components/ProductoForm";
import ProductoList from "./components/ProductoList";
import Carrito from "./components/Carrito";
import VentasList from "./components/VentasList";
import type { Producto, VentaDetalle } from "./types";
import { useProductos } from "./hooks/useProductos";
import { useVentas } from "./hooks/useVentas";
import { useCrearProducto } from "./hooks/useCrearProducto";
import { useGenerarVenta } from "./hooks/useGenerarVenta";
import { useAuth0 } from "@auth0/auth0-react";

function App() {
  const [carrito, setCarrito] = useState<VentaDetalle[]>([]);

  const {
    isLoading,
    isAuthenticated,
    loginWithRedirect: login,
    logout: auth0logout,
    user,
  } = useAuth0();

  const API_AUDIENCE = "https://my-api";

  const signup = () =>
    login({
      authorizationParams: {
        screen_hint: "sign up",
        audience: API_AUDIENCE,
      },
    });

  const handleLogin = async () => {
    console.log("Intentando login con audiencia:", API_AUDIENCE);
    try {
      await login({
        authorizationParams: {
          audience: API_AUDIENCE,
        },
      });
    } catch (error) {
      console.error("Error al redirigir:", error);
    }
  };

  const logout = () =>
    auth0logout({ logoutParams: { returnTo: window.location.origin } });

  const { data: productos = [], error: errorP } = useProductos();
  const { data: ventas = [], error: errorV } = useVentas();
  const crearProductoMutation = useCrearProducto();
  const generarVentaMutation = useGenerarVenta();

  const handleVenta = () => {
    if (carrito.length === 0) {
      alert("Carrito vacío");
      return;
    }

    generarVentaMutation.mutate({ detalles: carrito });

    setCarrito([]);
  };

  const handleCrearProducto = (producto: Producto) => {
    crearProductoMutation.mutate(producto);
  };

  const productoError =
    errorP?.message ||
    (crearProductoMutation.isError
      ? String(crearProductoMutation.error)
      : null);

  const ventaError =
    errorV?.message ||
    (generarVentaMutation.isError ? String(generarVentaMutation.error) : null);

  if (isLoading) return <div>Cargando autenticación...</div>;

  return (
    <div className="App">
      <h1>Inventory App</h1>
      {isAuthenticated ? (
        <div>
          <h1>Usuario: {user?.name}</h1>
          <button onClick={() => logout()}>Cerrar sesión</button>
        </div>
      ) : (
        <div>
          <button onClick={() => signup()}>Registrarse</button>
          <button onClick={() => handleLogin()}>Logearse</button>
        </div>
      )}

      {productoError && <p style={{ color: "red" }}>{productoError}</p>}
      {crearProductoMutation.isPending && <p>Creando producto...</p>}

      <ProductoForm onCrear={handleCrearProducto} />
      <ProductoList productos={productos} />

      <Carrito
        productos={productos}
        carrito={carrito}
        setCarrito={setCarrito}
        onVender={handleVenta}
      />

      {ventaError && <p style={{ color: "orange" }}>{ventaError}</p>}
      {generarVentaMutation.isPending && <p>Procesando venta...</p>}

      <VentasList ventas={ventas} productos={productos} />
    </div>
  );
}

export default App;
