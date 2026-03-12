package com.example.demo.controller;

import java.time.LocalDate;
import java.util.List;

import org.springframework.http.ResponseEntity;
import org.springframework.kafka.core.KafkaTemplate;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.example.demo.domain.Venta;
import com.example.demo.repository.VentaRepository;

import lombok.RequiredArgsConstructor;

@RestController
@CrossOrigin(origins = "*")
@RequestMapping("/api/venta")
@RequiredArgsConstructor
public class VentaController 
{
    private final VentaRepository _ventaRepository;
    private final KafkaTemplate<String, String> _kafkaTemplate;

    @GetMapping
    public ResponseEntity<List<Venta>> findAll() {
        return ResponseEntity.ok(_ventaRepository.findAll());
    }

    @PostMapping
    public ResponseEntity<Venta> realizarVenta(@RequestBody Venta venta) 
    {
        venta.setFecha(LocalDate.now());

        if (venta.getDetalles() != null)
            venta.getDetalles().forEach(d -> d.setVenta(venta));

        Venta saved = _ventaRepository.save(venta);

        _kafkaTemplate.send("venta-realizada", saved.toString());

        return ResponseEntity.ok(saved);
    }
}
