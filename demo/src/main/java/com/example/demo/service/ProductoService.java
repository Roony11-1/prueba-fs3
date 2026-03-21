package com.example.demo.service;

import java.util.List;

import org.springframework.http.HttpStatus;
// import org.springframework.kafka.core.KafkaTemplate;
import org.springframework.stereotype.Service;
import org.springframework.web.server.ResponseStatusException;

import com.example.demo.domain.Producto;
import com.example.demo.repository.ProductoRepository;

import io.github.resilience4j.retry.annotation.Retry;
import jakarta.transaction.Transactional;
import lombok.RequiredArgsConstructor;

@Service
@RequiredArgsConstructor
public class ProductoService {
    private final ProductoRepository _productoRepository;

    // Solo lectura, mantenemos el Retry por si la DB parpadea
    @Retry(name = "productoRetry", fallbackMethod = "fallbackFindAll")
    public List<Producto> findAll() {
        return _productoRepository.findAll();
    }

    @Transactional
    public Producto save(Producto producto) {
        return _productoRepository.save(producto);
    }

    public List<Producto> fallbackFindAll(Throwable e) {
        throw new ResponseStatusException(HttpStatus.SERVICE_UNAVAILABLE,
                "La base de datos del inventario no está disponible");
    }

    public boolean hayStock(int id, int cantidadRequerida) 
    {
        return _productoRepository.findById(id)
                .map(p -> p.getStock() >= cantidadRequerida)
                .orElse(false);
    }
}
