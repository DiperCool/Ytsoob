using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Payment.Subscriptions.Models;
using Ytsoob.Services.Payment.Ytsoobers.Features.Models;

namespace Ytsoob.Services.Payment.Shared.Contracts;

public interface IPaymentDbContext
{
    DbSet<TEntity> Set<TEntity>()
        where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DbSet<Ytsoober> Ytsoobers { get; }
    DbSet<Subscription> Subscriptions { get; }
}
