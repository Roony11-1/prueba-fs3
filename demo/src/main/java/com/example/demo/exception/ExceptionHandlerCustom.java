package com.example.demo.exception;

import java.util.Map;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;
import org.springframework.web.server.ResponseStatusException;

@RestControllerAdvice
public class ExceptionHandlerCustom
{
    @ExceptionHandler(ResponseStatusException.class)
    public ResponseEntity<?> handle(ResponseStatusException ex)
    {
        return ResponseEntity.status(ex.getStatusCode())
            .body(Map.of(
                "message", ex.getReason(),
                "status", ex.getStatusCode().value()
            ));
    }
}
