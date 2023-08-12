using BuildingBlocks.Persistence.EfCore.Postgres;

namespace Ytsoob.Services.Payment.Shared.Data;

public class CustomerDbContextDesignFactory : DbContextDesignFactoryBase<PaymentDbContext>
{
    public CustomerDbContextDesignFactory()
        : base("PostgresOptions:ConnectionString") { }
}
