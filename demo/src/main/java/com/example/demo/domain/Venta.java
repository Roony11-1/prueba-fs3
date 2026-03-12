package com.example.demo.domain;

import java.time.LocalDate;
import java.util.List;

import com.fasterxml.jackson.annotation.JsonManagedReference;

import jakarta.persistence.CascadeType;
import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.OneToMany;
import lombok.Data;
import lombok.NoArgsConstructor;

@Entity
@Data
@NoArgsConstructor
public class Venta 
{
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer id;
    @OneToMany(mappedBy = "venta", cascade = CascadeType.ALL, fetch = FetchType.EAGER)
    @JsonManagedReference
    private List<VentaDetalle> detalles;
    private LocalDate fecha;

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();

        sb.append(this.id).append(":");

        for (int i = 0; i < detalles.size(); i++)
        {
            if (i > 0)
                sb.append(";");

            sb.append(detalles.get(i).toString());
        }

        return sb.toString();
    }
}
