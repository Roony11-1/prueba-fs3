package com.example.demo.service;

import java.util.List;

import org.springframework.http.HttpStatus;
import org.springframework.kafka.core.KafkaTemplate;
import org.springframework.stereotype.Service;
import org.springframework.web.server.ResponseStatusException;

import com.example.demo.domain.Producto;
import com.example.demo.repository.ProductoRepository;

import io.github.resilience4j.retry.annotation.Retry;
import lombok.RequiredArgsConstructor;

@Service
@RequiredArgsConstructor
public class ProductoService 
{
    private final ProductoRepository _productoRepository;
    private final KafkaTemplate<String, Producto> _kafkaTemplate;

    @Retry(name = "productoRetry", fallbackMethod = "fallbackFindAll")
    public List<Producto> findAll() 
    {
        return _productoRepository.findAll();
    }

    public List<Producto> fallbackFindAll(Throwable e) 
    {
        throw new ResponseStatusException(HttpStatus.SERVICE_UNAVAILABLE,
                "La base de datos del inventario no está disponible");
    }

    @Retry(name = "productoRetry", fallbackMethod = "fallbackSave")
    public Producto save(Producto producto) 
    {
        return _productoRepository.save(producto);
    }

    public Producto fallbackSave(Producto producto, Throwable e) 
    {
        /*try 
        {
            _kafkaTemplate.send("productos-pendientes", producto);
            return producto;
        } 
        catch (Exception ex) 
        {*/
            // Kafka también falló
            throw new ResponseStatusException(
                    HttpStatus.SERVICE_UNAVAILABLE,
                    "No se pudo guardar ni en DB ni en cola");
        //}
        // Por algun motivo no manda el mensaje con ese "handler" solo si la mando cruda
        // mañana preguntarle al freddy si alguna idea?
    }
}
