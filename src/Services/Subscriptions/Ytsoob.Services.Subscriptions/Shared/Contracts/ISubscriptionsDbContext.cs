using Microsoft.EntityFrameworkCore;

namespace Ytsoob.Services.Subscriptions.Shared.Contracts;

public interface ISubscriptionsDbContext
{
    DbSet<TEntity> Set<TEntity>()
        where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
