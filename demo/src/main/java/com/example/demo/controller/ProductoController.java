package com.example.demo.controller;

import java.util.List;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.example.demo.domain.Producto;
import com.example.demo.repository.ProductoRepository;

import lombok.RequiredArgsConstructor;

@RestController
@CrossOrigin(origins = "*")
@RequestMapping("/api/producto")
@RequiredArgsConstructor
public class ProductoController 
{
    private final ProductoRepository _productoRepository;

    @GetMapping
    public ResponseEntity<List<Producto>> findAll()
    {
        return ResponseEntity.ok(_productoRepository.findAll());
    }

    @PostMapping
    public ResponseEntity<Producto> save(@RequestBody Producto producto)
    {
        return ResponseEntity.ok(_productoRepository.save(producto));
    }
}
