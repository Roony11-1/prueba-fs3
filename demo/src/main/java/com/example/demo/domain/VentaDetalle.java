package com.example.demo.domain;

import com.fasterxml.jackson.annotation.JsonBackReference;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import lombok.Data;

@Entity
@Data
public class VentaDetalle 
{
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer id;

    private int productoId;

    private int cantidad;

    @ManyToOne
    @JoinColumn(name = "venta_id")
    @JsonBackReference
    private Venta venta;

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();

        // Quiero separar cada campo para que lea id-cantidad

        sb.append(this.productoId+"-"+this.cantidad);

        return sb.toString();
    }
}
