using BuildingBlocks.Core.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Subscriptions.Shared.Contracts;
using Ytsoob.Services.Subscriptions.Subscriptions.Models;
using Ytsoob.Services.Subscriptions.Ytsoobers.Models;

namespace Ytsoob.Services.Subscriptions.Shared.Data;

public class SubscriptionsDbContext : EfDbContextBase, ISubscriptionsDbContext
{
    public const string DefaultSchema = "subscriptions";

    public SubscriptionsDbContext(DbContextOptions<SubscriptionsDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension(EfConstants.UuidGenerator);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Ytsoober> Ytsoobers => Set<Ytsoober>();
}
