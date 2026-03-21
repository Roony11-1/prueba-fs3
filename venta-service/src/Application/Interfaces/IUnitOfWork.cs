using VentaService.Domain;

namespace VentaService.Application.Interfaces;

public interface IUnitOfWork
{
    Task AddOutboxMessageAsync(OutboxMessage message);
    Task CommitAsync(CancellationToken cancellationToken = default);
}