package com.example.demo.controller;

import java.util.List;
import java.util.Objects;

import org.springframework.kafka.annotation.KafkaListener;
import org.springframework.stereotype.Service;

import com.example.demo.domain.Producto;
import com.example.demo.domain.event.VentaRealizadaEvent;
import com.example.demo.repository.ProductoRepository;

import lombok.RequiredArgsConstructor;
import tools.jackson.databind.ObjectMapper;

@Service
@RequiredArgsConstructor
public class InventoryConsumer 
{
    private final ProductoRepository _productoRepository;
    private final ObjectMapper _mapper;

    @KafkaListener(topics = "venta-realizada", groupId = "producto-group")
    public void descontarInventario(String message)
    {
        try 
        {
            VentaRealizadaEvent event = _mapper.readValue(message, VentaRealizadaEvent.class);

            System.out.println("Descontando inventario para venta " + event.getId());

            List<Producto> productos = event.getDetalles().stream()
                    .map(detalle -> _productoRepository.findById(detalle.getProductoId())
                            .map(p -> {
                                p.setStock(p.getStock() - detalle.getCantidad());
                                return p;
                            })
                            .orElse(null))
                    .filter(Objects::nonNull)
                    .toList();

            _productoRepository.saveAll(productos);

            System.out.println("Cantidad de productos actualizados: " + productos.size());
        } 
        catch (Exception e) 
        {
            System.err.println("Error procesando mensaje Kafka: " + message);
            e.printStackTrace();
        }
    }
}
