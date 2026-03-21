using System.Text.Json;
using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VentaService.Application;
using VentaService.Domain;
using VentaService.Infrastructure.Messaging;
using VentaService.Infrastructure.Persistence;

namespace VentaService.Infrastructure.BackgroundJobs;

public class OutboxProcessorBackgroundService(
    IServiceProvider serviceProvider,
    Channel<bool> outboxChannel,
    ILogger<OutboxProcessorBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int failCount = 0;

        // Se activa cuando lee un mensaje "bool"
        while (await outboxChannel.Reader.WaitToReadAsync(stoppingToken))
        {
            while (outboxChannel.Reader.TryRead(out _))
            {
                try
                {
                    await ProcessOutboxMessagesAsync(stoppingToken);
                    failCount = 0;
                }
                catch (Exception ex)
                {
                    failCount++;
                    logger.LogError(ex, "Error en Outbox. Intento de reintento: {Count}", failCount);

                    // Espera exponencial: 2s, 4s, 8s... hasta un máximo de 30s
                    var delay = Math.Min(Math.Pow(2, failCount), 30);
                    await Task.Delay(TimeSpan.FromSeconds(delay), stoppingToken);
                }
            }

            // Opcional: un pequeño delay de seguridad para evitar bucles infinitos en errores
            await Task.Delay(100, stoppingToken);
        }
    }

    private async Task ProcessOutboxMessagesAsync(CancellationToken stoppingToken)
    {
        // Usamos un Scope porque el Worker es Singleton y el DbContext es Scoped
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var eventPublisher = scope.ServiceProvider.GetRequiredService<IEventPublisher>();

        // Buscamos mensajes no procesados (lote de 20 para no saturar memoria)
        var messages = dbContext.Set<OutboxMessage>()
            .Where(m => m.ProcessedOn == null)
            .Take(20)
            .ToList();

        if (messages.Count == 0) return;

        foreach (var message in messages)
        {
            try
            {
                // Aquí publicas a Kafka. ¡Si falla, lanzará excepción y no se marcará como procesado!
                await eventPublisher.PublishRawAsync(message.Type, message.Payload);

                message.ProcessedOn = DateTimeOffset.UtcNow;
                message.Error = null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error enviando mensaje {Id} a Kafka", message.Id);
                message.Error = ex.Message;
            }
        }

        await dbContext.SaveChangesAsync(stoppingToken);
    }
}