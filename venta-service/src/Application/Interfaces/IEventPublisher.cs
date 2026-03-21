namespace VentaService.Application;

public interface IEventPublisher
{
    Task PublishAsync<T>(string topic, T message);
    Task PublishRawAsync(string topic, string rawMessage);
    void Dispose();
}