namespace VentaService.Domain;

public class OutboxMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Type { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
    public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? ProcessedOn { get; set; }
    public string? Error { get; set; }
}