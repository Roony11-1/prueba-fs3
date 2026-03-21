namespace VentaService.Domain.CustomExceptions;

public class ServiceUnavailableException(string message) : Exception(message);