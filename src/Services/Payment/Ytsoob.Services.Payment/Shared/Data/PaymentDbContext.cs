using BuildingBlocks.Core.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Payment.Shared.Contracts;
using Ytsoob.Services.Payment.Subscriptions.Models;
using Ytsoob.Services.Payment.Ytsoobers.Features.Models;

namespace Ytsoob.Services.Payment.Shared.Data;

public class PaymentDbContext : EfDbContextBase, IPaymentDbContext
{
    public const string DefaultSchema = "payment";

    public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension(EfConstants.UuidGenerator);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Ytsoober> Ytsoobers => Set<Ytsoober>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<PriceProduct> PriceProducts => Set<PriceProduct>();
}
