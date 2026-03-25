import { useState } from "react";
import "./App.css";

import ProductoForm from "./components/ProductoForm";
import ProductoList from "./components/ProductoList";
import Carrito from "./components/Carrito";
import VentasList from "./components/VentasList";
import { useKeycloak } from "./auth/useKeyCloak";
import type { Producto, VentaDetalle } from "./types";
import { useProductos } from "./hooks/useProductos";
import { useVentas } from "./hooks/useVentas";
import { useCrearProducto } from "./hooks/useCrearProducto";
import { useGenerarVenta } from "./hooks/useGenerarVenta";

function App() {
  const [carrito, setCarrito] = useState<VentaDetalle[]>([]);

  const { keycloak, authenticated } = useKeycloak();
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

  const handleLogOut = () => {
    if (keycloak) keycloak?.logout();
  };

  if (!keycloak) return <div>Cargando...</div>;

  const productoError =
    errorP?.message ||
    (crearProductoMutation.isError
      ? String(crearProductoMutation.error)
      : null);

  const ventaError =
    errorV?.message ||
    (generarVentaMutation.isError ? String(generarVentaMutation.error) : null);

  return (
    <div className="App">
      <h1>Inventory App</h1>

      {authenticated && (
        <>
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

          <button onClick={handleLogOut}>Cerrar sesión</button>
        </>
      )}
    </div>
  );
}

export default App;
