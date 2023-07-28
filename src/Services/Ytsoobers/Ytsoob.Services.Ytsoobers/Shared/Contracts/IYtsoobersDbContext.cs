using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Ytsoobers.Ytsoobers.Models;

namespace Ytsoob.Services.Ytsoobers.Shared.Contracts;

public interface IYtsoobersDbContext
{
    DbSet<Ytsoober> Ytsoobers { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
