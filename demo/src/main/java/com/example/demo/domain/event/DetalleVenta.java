package com.example.demo.domain.event;


import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class DetalleVenta 
{
    private Integer id;
    private Integer productoId;
    private Integer cantidad;
    private Integer ventaId;
}