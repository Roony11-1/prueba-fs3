using Confluent.Kafka;

namespace VentaService.Infrastructure.Messaging;

public class KafkaProducer(string bootstrapServers)
{
    private readonly string _bootstrapServers = bootstrapServers;

    public async Task EnviarMensajeAsync(string topic, string message)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = _bootstrapServers
        };

        using var producer = new ProducerBuilder<Null, string>(config).Build();

        try
        {
            var result = await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });

            producer.Flush(TimeSpan.FromSeconds(5));

            Console.WriteLine($"Mensaje enviado a {result.TopicPartitionOffset}");
            Console.WriteLine($"Mensaje: {message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al enviar: {ex.Message}");
        }
    }
}