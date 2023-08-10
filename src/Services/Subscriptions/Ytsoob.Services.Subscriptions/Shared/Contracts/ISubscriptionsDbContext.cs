using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Subscriptions.Subscriptions.Models;
using Ytsoob.Services.Subscriptions.Ytsoobers.Models;

namespace Ytsoob.Services.Subscriptions.Shared.Contracts;

public interface ISubscriptionsDbContext
{
    DbSet<TEntity> Set<TEntity>()
        where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DbSet<Subscription> Subscriptions { get; }
    DbSet<Ytsoober> Ytsoobers { get; }
}
