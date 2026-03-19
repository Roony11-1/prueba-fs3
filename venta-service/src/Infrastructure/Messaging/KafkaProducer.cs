using Confluent.Kafka;

namespace VentaService.Infrastructure.Messaging;

public class KafkaProducer(string bootstrapServers)
{
    private readonly string _bootstrapServers = bootstrapServers;

    public async Task EnviarMensajeAsync(string topic, string message)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = _bootstrapServers,

            MessageTimeoutMs = 1500,
            SocketTimeoutMs = 1500,
            RequestTimeoutMs = 1500
        };

        using var producer = new ProducerBuilder<Null, string>(config).Build();

        try
        {
            var result = await producer.ProduceAsync(topic, new Message<Null, string>
            {
                Value = message
            });

            if (result.Status != PersistenceStatus.Persisted)
            {
                throw new Exception("Kafka no confirmó el mensaje");
            }

            producer.Flush(TimeSpan.FromSeconds(5));

            Console.WriteLine($"Mensaje enviado a {result.TopicPartitionOffset}");
            Console.WriteLine($"Mensaje: {message}");
        }
        catch (ProduceException<Null, string> ex)
        {
            throw new Exception("KAFKA_ERROR: " + ex.Error.Reason, ex);
        }
        catch (Exception ex)
        {
            throw new Exception("KAFKA_ERROR: " + ex.Message, ex);
        }
    }
}