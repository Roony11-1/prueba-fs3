import { useState, useEffect } from 'react';
import './App.css';

interface Producto
{
  id: number;
  nombre: string;
  precio: number;
  stock: number;
}

function App() {
  const [productos, setProductos] = useState<Producto[]>([]);
  const [nombre, setNombre] = useState('');
  const [precio, setPrecio] = useState('');
  const [stock, setStock] = useState('');

  const apiUrl = 'http://localhost:5048/api/producto';

  // Traer todos los productos al iniciar
  useEffect(() => {
    fetch(apiUrl)
      .then(res => res.json())
      .then(data => setProductos(data))
      .catch(err => console.error(err));
  }, []);

  // Crear un producto nuevo
  const crearProducto = async () => {
    if (!nombre || !precio || !stock) {
      alert('Todos los campos son obligatorios');
      return;
    }

    const nuevoProducto = {
      nombre,
      precio: parseFloat(precio),
      stock: parseInt(stock)
    };

    try {
      const res = await fetch(apiUrl, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(nuevoProducto)
      });

      if (!res.ok) throw new Error('Error al crear producto');

      const creado = await res.json();
      setProductos([...productos, creado]);

      // Limpiar formulario
      setNombre('');
      setPrecio('');
      setStock('');
    } catch (err) {
      console.error(err);
      alert('Error creando producto');
    }
  };

  return (
    <div className="App">
      <h1>Inventory App</h1>

      <h2>Crear Producto</h2>
      <input
        placeholder="Nombre"
        value={nombre}
        onChange={e => setNombre(e.target.value)}
      />
      <input
        type="number"
        placeholder="Precio"
        value={precio}
        onChange={e => setPrecio(e.target.value)}
      />
      <input
        type="number"
        placeholder="Stock"
        value={stock}
        onChange={e => setStock(e.target.value)}
      />
      <button onClick={crearProducto}>Guardar</button>

      <h2>Productos</h2>
      <ul>
        {productos.map(p => (
          <li key={p.id}>
            {p.nombre} - ${p.precio} - Stock: {p.stock}
          </li>
        ))}
      </ul>
    </div>
  );
}

export default App;