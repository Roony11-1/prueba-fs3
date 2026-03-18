export interface Producto {
  id: number;
  nombre: string;
  precio: number;
  stock: number;
}

export interface VentaDetalle {
  id?: number;
  productoId: number;
  cantidad: number;
}

export interface Venta {
  id?: number;
  detalles: VentaDetalle[];
  fecha?: string;
}