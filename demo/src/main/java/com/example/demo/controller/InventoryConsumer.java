package com.example.demo.controller;

import java.util.ArrayList;
import java.util.List;

import org.springframework.kafka.annotation.KafkaListener;
import org.springframework.stereotype.Service;

import com.example.demo.domain.Producto;
import com.example.demo.repository.ProductoRepository;

import lombok.RequiredArgsConstructor;

@Service
@RequiredArgsConstructor
public class InventoryConsumer 
{
    private final ProductoRepository _productoRepository;

    @KafkaListener(topics = "venta-realizada", groupId = "producto-group")
    public void descontarInventario(String message)
    {
        System.out.println(message);
        
        List<String> parts = List.of(message.split(":"));

        // El id de la venta:
        Integer ventaId = Integer.parseInt(parts.get(0));

        // [pId-C, pId-C, ..., pId-C]
        List<String> detalles = List.of(parts.get(1).split(";"));

        List<Integer> productoIds = new ArrayList<>();
        List<Integer> cantidades = new ArrayList<>();

        detalles.forEach(d -> 
        {
            List<String> detalleParts = List.of(d.split("-"));

            productoIds.add(Integer.parseInt(detalleParts.get(0)));
            cantidades.add(Integer.parseInt(detalleParts.get(1)));
        });

        List<Producto> prodUpdate = new ArrayList<>();

        for (int i = 0; i < productoIds.size(); i++)
        {
            Integer pId = productoIds.get(i);
            Integer cantidad = cantidades.get(i);

            _productoRepository.findById(pId).ifPresent(p -> 
            {
                p.setStock(p.getStock() - cantidad);
                prodUpdate.add(p);
            });
        }

        System.out.println("Descontando inventario para venta "+ventaId);
        System.out.println("Cantidad de productos actualizados: "+prodUpdate.size());

        _productoRepository.saveAll(prodUpdate);
    }
}
