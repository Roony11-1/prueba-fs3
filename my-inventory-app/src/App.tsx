import { useEffect, useState } from "react";
import "./App.css";

import type { Producto, Venta, VentaDetalle } from "./types";
import { obtenerProductos, crearProducto } from "./services/productoService";
import { obtenerVentas, crearVenta } from "./services/ventaService";

import ProductoForm from "./components/ProductoForm";
import ProductoList from "./components/ProductoList";
import Carrito from "./components/Carrito";
import VentasList from "./components/VentasList";
import Keycloak from "keycloak-js";

const keycloakOptions={
  url:"http://localhost:8080",
  realm: "reino-neytan",
  clientId: "ricardito"
}

function App() 
{
  const [productos, setProductos] = useState<Producto[]>([]);
  const [ventas, setVentas] = useState<Venta[]>([]);
  const [carrito, setCarrito] = useState<VentaDetalle[]>([]);

  const [errorP, setErrorP] = useState<string | null>(null);
  const [errorV, setErrorV] = useState<string | null>(null);

  const cargarProductos = async () => {
    try {
      setErrorP(null);
      const prod = await obtenerProductos();
      setProductos(prod);
    } catch (err: any) {
      console.error(err);
      setErrorP(err.message);
    }
  };

  const cargarVentas = async () => {
    try {
      setErrorV(null);
      const vent = await obtenerVentas();
      setVentas(vent);
    } catch (err: any) {
      console.error(err);
      setErrorV(err.message);
    }
  };

  const handleCrearProducto = async (producto: any) => {
    try {
      setErrorP(null);

      await crearProducto(producto);

      await cargarProductos();
    } catch (err: any) {
      console.error(err);
      setErrorP(err.message);
    }
  };

  const handleVenta = async () => {
    if (carrito.length === 0) {
      alert("Carrito vacío");
      return;
    }

    try {
      setErrorV(null);

      await crearVenta({ detalles: carrito });

      setCarrito([]);

      await cargarVentas();
      await cargarProductos();
    } catch (err: any) {
      console.error(err);
      setErrorV(err.message);
    }
  };

  const [keycloak, setKeyCloack] = useState<Keycloak | null>(null);

  useEffect( () => 
  {
    const initKetCloack = async () => 
    {
      const keyCloackInstance = new Keycloak(keycloakOptions)
      try {
        await keyCloackInstance.init({ onLoad: "login-required" })
        setKeyCloack(keyCloackInstance)
      } catch (error) {
        console.log("error: "+error)
      }
    }
    initKetCloack()
  }, []);

  const handleLogOut = () => 
  {
    if (keycloak)
      keycloak?.logout()
  }

  useEffect(() => {
  if (keycloak?.authenticated) {
    cargarProductos();
    cargarVentas();
  }
}, [keycloak]);

if (!keycloak) return <div>Cargando...</div>;

return (
  <div className="App">
    <h1>Inventory App</h1>

    {keycloak.authenticated && (
      <>
        {errorP && <div style={{ color: "red" }}>{errorP}</div>}

        <ProductoForm onCrear={handleCrearProducto} />
        <ProductoList productos={productos} />

        <Carrito
          productos={productos}
          carrito={carrito}
          setCarrito={setCarrito}
          onVender={handleVenta}
        />

        {errorV && <div style={{ color: "orange" }}>{errorV}</div>}

        <VentasList ventas={ventas} productos={productos} />

        <button onClick={handleLogOut}>Cerrar sesión</button>
      </>
    )}
  </div>
);
}

export default App;