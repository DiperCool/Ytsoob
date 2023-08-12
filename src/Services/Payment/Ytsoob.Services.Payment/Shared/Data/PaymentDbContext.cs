using BuildingBlocks.Core.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Payment.Shared.Contracts;

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
}
