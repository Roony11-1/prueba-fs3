using Confluent.Kafka;
using VentaService.Application;
using System.Text.Json;

namespace VentaService.Infrastructure.Messaging;

public class KafkaProducer : IEventPublisher
{
    private readonly IProducer<Null, string> _producer;
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public KafkaProducer(string bootstrapServers)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = bootstrapServers
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task PublishAsync<T>(string topic, T message)
    {
        var json = JsonSerializer.Serialize(message, _jsonOptions);

        await _producer.ProduceAsync(topic, new Message<Null, string>
        {
            Value = json
        });
    }
}