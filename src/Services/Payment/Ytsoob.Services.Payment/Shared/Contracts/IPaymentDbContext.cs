using Microsoft.EntityFrameworkCore;

namespace Ytsoob.Services.Payment.Shared.Contracts;

public interface IPaymentDbContext
{
    DbSet<TEntity> Set<TEntity>()
        where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
