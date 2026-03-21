package com.example.demo.controller;

import java.util.List;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import com.example.demo.domain.Producto;
import com.example.demo.service.ProductoService;

import lombok.RequiredArgsConstructor;

@RestController
@CrossOrigin(origins = "*", allowedHeaders = "*")
@RequestMapping("/api/producto")
@RequiredArgsConstructor
public class ProductoController 
{
    private final ProductoService _productoService;

    @GetMapping
    public ResponseEntity<List<Producto>> findAll() 
    {
        return ResponseEntity.ok(_productoService
            .findAll());
    }

    @PostMapping
    public ResponseEntity<Producto> save(@RequestBody Producto producto) 
    {
        return ResponseEntity.ok(_productoService
            .save(producto));
    }

    @GetMapping("/{id}/validar-stock")
    public ResponseEntity<Boolean> validarStock(@PathVariable int id, @RequestParam int cantidad) 
    {
        boolean disponible = _productoService.hayStock(id, cantidad);
        return ResponseEntity.ok(disponible);
    }
}
