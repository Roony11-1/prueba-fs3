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

  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    cargarDatos();
  }, []);

  const cargarDatos = async () => {
    try {
      setError(null);

      const prod = await obtenerProductos();
      const vent = await obtenerVentas();

      setProductos(prod);
      setVentas(vent);
    } catch (err: any) {
      console.error(err);
      setError(err.message);
    }
  };

  const handleCrearProducto = async (producto: any) => {
    try {
      setError(null);

      const creado = await crearProducto(producto);
      setProductos([...productos, creado]);

    } catch (err: any) {
      console.error(err);
      setError(err.message);
    }
  };

  const handleVenta = async () => {
    if (carrito.length === 0) {
      alert("Carrito vacío");
      return;
    }

    try {
      const mensaje = await crearVenta({ detalles: carrito });
      alert(mensaje);
      setCarrito([]);
      cargarDatos();
    } catch (err: any) {
      alert(err.message);
    }
  };

  return (
    <div className="App">
      <h1>Inventory App</h1>
      {error && <div style={{ color: "red" }}>{error}</div>}
      <ProductoForm onCrear={handleCrearProducto} />
      <ProductoList productos={productos} />

      <Carrito
        productos={productos}
        carrito={carrito}
        setCarrito={setCarrito}
        onVender={handleVenta}
      />

      <VentasList ventas={ventas} productos={productos} />
    </div>
  );
}

export default App;
