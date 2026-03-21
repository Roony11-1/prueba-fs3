import { useEffect, useState } from "react";
import "./App.css";

import type { Producto, Venta, VentaDetalle } from "./types";
import { obtenerProductos, crearProducto } from "./services/productoService";
import { obtenerVentas, crearVenta } from "./services/ventaService";

import ProductoForm from "./components/ProductoForm";
import ProductoList from "./components/ProductoList";
import Carrito from "./components/Carrito";
import VentasList from "./components/VentasList";

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

  useEffect(() => {
    cargarProductos();
    cargarVentas();
  }, []);

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

  useEffect(() => {
    const interval = setInterval(() => {
      cargarProductos();
    }, 3000); // cada 3 segundos

    return () => clearInterval(interval);
  }, []);

  return (
    <div className="App">
      <h1>Inventory App</h1>

      {/* errores productos */}
      {errorP && <div style={{ color: "red" }}>{errorP}</div>}

      <ProductoForm onCrear={handleCrearProducto} />
      <ProductoList productos={productos} />

      <Carrito
        productos={productos}
        carrito={carrito}
        setCarrito={setCarrito}
        onVender={handleVenta}
      />

      {/* errores ventas */}
      {errorV && <div style={{ color: "orange" }}>{errorV}</div>}

      <VentasList ventas={ventas} productos={productos} />
    </div>
  );
}

export default App;