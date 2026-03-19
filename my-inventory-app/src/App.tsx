import { useEffect, useState } from "react";
import "./App.css";

import type { Producto, Venta, VentaDetalle } from "./types";
import { obtenerProductos, crearProducto } from "./services/productoService";
import { obtenerVentas, crearVenta } from "./services/ventaService";

import ProductoForm from "./components/ProductoForm";
import ProductoList from "./components/ProductoList";
import Carrito from "./components/Carrito";
import VentasList from "./components/VentasList";

function App() {
  const [productos, setProductos] = useState<Producto[]>([]);
  const [ventas, setVentas] = useState<Venta[]>([]);
  const [carrito, setCarrito] = useState<VentaDetalle[]>([]);

  const [errorP, setErrorP] = useState<string | null>(null);
  const [errorV, setErrorV] = useState<string | null>(null);

  useEffect(() => {
    cargarDatos();
  }, []);

  const cargarDatos = async () => {
    setErrorP(null);
    setErrorV(null);

    try {
      const prod = await obtenerProductos();
      setProductos(prod);
    } catch (err: any) {
      console.error(err);
      setErrorP(err.message);
    }

    try {
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

      const creado = await crearProducto(producto);
      setProductos([...productos, creado]);
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

      const venta = await crearVenta({ detalles: carrito });

      setCarrito([]);
      setVentas([...ventas, venta]);
    } catch (err: any) {
      console.error(err);
      setErrorV(err.message);
    }
  };

  return (
    <div className="App">
      <h1>Inventory App</h1>
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
    </div>
  );
}

export default App;
