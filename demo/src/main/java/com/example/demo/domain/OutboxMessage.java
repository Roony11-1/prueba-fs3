package com.example.demo.domain;

import java.time.LocalDateTime;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Entity
@Table(name = "outbox_messages")
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class OutboxMessage 
{
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    private String type;
    @Column(columnDefinition = "TEXT")
    private String payload;
    private final LocalDateTime createdAt = LocalDateTime.now();
    private LocalDateTime processedAt;
}
