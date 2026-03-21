namespace VentaService.Infrastructure.Persistence;
using VentaService.Application.Interfaces;
using VentaService.Domain;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    public async Task AddOutboxMessageAsync(OutboxMessage message)
    {
        await dbContext.Set<OutboxMessage>().AddAsync(message);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken); 
    }
}