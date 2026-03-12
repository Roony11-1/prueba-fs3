import { useState, useEffect } from "react";
import "./App.css";

interface Producto {
  id: number;
  nombre: string;
  precio: number;
  stock: number;
}

interface VentaDetalle {
  id?: number;
  productoId: number;
  cantidad: number;
}

interface Venta {
  id?: number;
  detalles: VentaDetalle[];
  fecha?: string;
}

function App() {
  const [productos, setProductos] = useState<Producto[]>([]);
  const [ventas, setVentas] = useState<Venta[]>([]);
  const [nombre, setNombre] = useState("");
  const [precio, setPrecio] = useState("");
  const [stock, setStock] = useState("");

  const [carrito, setCarrito] = useState<VentaDetalle[]>([]);

  const apiUrl = "http://localhost:5000/api/producto";

  // Traer todos los productos al iniciar
  useEffect(() => {
    fetch(apiUrl)
      .then((res) => res.json())
      .then((data) => setProductos(data))
      .catch((err) => console.error(err));

    fetch("http://localhost:5000/api/venta")
      .then((res) => res.json())
      .then((data) => setVentas(data))
      .catch((err) => console.error(err));
  }, []);

  const handleVenta = () => {
    const venta: Venta = {
      detalles: carrito,
    };

    if (carrito.length === 0) {
      alert("El carrito está vacío. Agrega productos antes de vender.");
      return;
    }

    fetch("http://localhost:5000/api/venta", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(venta),
    })
      .then((res) => {
        if (!res.ok) throw new Error("Error al realizar la venta");

        return res.text();
      })
      .then((data) => {
        alert(data);
        setCarrito([]);
      })
      .catch((err) => {
        alert(err.message); // muestra el error
      });
  };

  // Crear un producto nuevo
  const crearProducto = async () => {
    if (!nombre || !precio || !stock) {
      alert("Todos los campos son obligatorios");
      return;
    }

    const nuevoProducto = {
      nombre,
      precio: parseFloat(precio),
      stock: parseInt(stock),
    };

    try {
      const res = await fetch(apiUrl, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(nuevoProducto),
      });

      if (!res.ok) throw new Error("Error al crear producto");

      const creado = await res.json();
      setProductos([...productos, creado]);

      // Limpiar formulario
      setNombre("");
      setPrecio("");
      setStock("");
    } catch (err) {
      console.error(err);
      alert("Error creando producto");
    }
  };

  return (
    <div className="App">
      <h1>Inventory App</h1>

      <h2>Crear Producto</h2>
      <input
        placeholder="Nombre"
        value={nombre}
        onChange={(e) => setNombre(e.target.value)}
      />
      <input
        type="number"
        placeholder="Precio"
        value={precio}
        onChange={(e) => setPrecio(e.target.value)}
      />
      <input
        type="number"
        placeholder="Stock"
        value={stock}
        onChange={(e) => setStock(e.target.value)}
      />
      <button onClick={crearProducto}>Guardar</button>

      <h2>Productos</h2>
      <ul>
        {productos.map((p) => (
          <li key={p.id}>
            {p.nombre} - ${p.precio} - Stock: {p.stock}
          </li>
        ))}
      </ul>

      <hr />
      <h1>Zona de ventas</h1>
      <h3>Productos Disponibles</h3>
      {productos.map((p) => {
        const carritoItem = carrito.find((item) => item.productoId === p.id);
        const estaEnCarrito = !!carritoItem;

        return (
          <div
            key={p.id}
            style={{
              border: "1px solid #ccc",
              borderRadius: "8px",
              padding: "10px 15px",
              marginBottom: "10px",
              backgroundColor: "#0f0202",
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              flexDirection: "column",
            }}
          >
            <div
              style={{
                display: "flex",
                justifyContent: "space-between",
                width: "100%",
                alignItems: "center",
              }}
            >
              <span>
                {p.nombre} - ${p.precio} - Stock: {p.stock}
              </span>
              <button
                style={{
                  padding: "5px 10px",
                  backgroundColor: estaEnCarrito ? "#f44336" : "#4CAF50",
                  color: "white",
                  border: "none",
                  borderRadius: "4px",
                  cursor: "pointer",
                }}
                onClick={() => {
                  if (estaEnCarrito) {
                    setCarrito((prev) =>
                      prev.filter((item) => item.productoId !== p.id),
                    );
                  } else {
                    setCarrito((prev) => [
                      ...prev,
                      { productoId: p.id, cantidad: 1 },
                    ]);
                  }
                }}
              >
                {estaEnCarrito ? "Quitar" : "Añadir"}
              </button>
            </div>

            {estaEnCarrito && (
              <input
                type="number"
                min={1}
                max={p.stock}
                value={carritoItem.cantidad}
                onChange={(e) => {
                  const cantidad = Math.min(
                    Math.max(1, Number(e.target.value)),
                    p.stock,
                  );
                  setCarrito((prev) =>
                    prev.map((item) =>
                      item.productoId === p.id ? { ...item, cantidad } : item,
                    ),
                  );
                }}
                style={{
                  marginTop: "10px",
                  padding: "5px",
                  width: "60px",
                  borderRadius: "4px",
                }}
              />
            )}
          </div>
        );
      })}
      <button onClick={handleVenta}> Vender </button>
      <hr />
      <h1>Ventas realizada</h1>
      {ventas.map((v) => (
        <div
          key={v.id}
          style={{
            marginBottom: "20px",
            padding: "15px",
            borderRadius: "10px",
            backgroundColor: "#1f1f1f",
            color: "white",
            boxShadow: "0 4px 6px rgba(0,0,0,0.3)",
          }}
        >
          <h3 style={{ margin: "0 0 10px 0", color: "#FFD700" }}>
            Venta #{v.id}
          </h3>
          <h4 style={{ margin: "0 0 10px 0", color: "#FFB347" }}>Detalles:</h4>

          <div style={{ display: "flex", flexDirection: "column", gap: "6px" }}>
            {v.detalles.map((d) => {
              const producto = productos.find((p) => p.id === d.productoId);
              return (
                <div
                  key={d.productoId}
                  style={{
                    padding: "6px 10px",
                    borderRadius: "6px",
                    backgroundColor: "#2c2c2c",
                    display: "flex",
                    justifyContent: "space-between",
                    alignItems: "center",
                  }}
                >
                  {producto ? (
                    <span>{producto.nombre}</span>
                  ) : (
                    <span>Producto ID {d.productoId}</span>
                  )}
                  <span style={{ fontWeight: "bold", color: "#4CAF50" }}>
                    Cantidad: {d.cantidad}
                  </span>
                </div>
              );
            })}
          </div>

          <div
            style={{ marginTop: "10px", fontStyle: "italic", color: "#ccc" }}
          >
            Fecha: {v.fecha}
          </div>
        </div>
      ))}
    </div>
  );
}

export default App;
