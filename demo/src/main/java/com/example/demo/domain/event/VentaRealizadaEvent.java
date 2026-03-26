package com.example.demo.domain.event;

import java.time.Instant;
import java.util.List;


import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class VentaRealizadaEvent 
{
    private Integer id;
    private String usuarioId;
    private List<DetalleVenta> detalles;
    private Instant fecha;
}
