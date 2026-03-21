using Confluent.Kafka;
using VentaService.Application;
using System.Text.Json;
using VentaService.Infrastructure.Common;

namespace VentaService.Infrastructure.Messaging;

public class KafkaProducer : IEventPublisher
{
    private readonly IProducer<Null, string> _producer;

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
        var json = JsonSerializer.Serialize(message,  JsonDefaults.CamelCaseOptions);
        
        await _producer.ProduceAsync(topic, new Message<Null, string>
        {
            Value = json
        });
    }

    public async Task PublishRawAsync(string topic, string rawMessage)
    {
        await _producer.ProduceAsync(topic, new Message<Null, string>
        {
            Value = rawMessage
        });
    }

    public void Dispose()
    {
        _producer?.Flush();
        _producer?.Dispose();
    }
}