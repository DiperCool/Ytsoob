using BuildingBlocks.Persistence.EfCore.Postgres;

namespace Ytsoob.Services.Subscriptions.Shared.Data;

public class CustomerDbContextDesignFactory : DbContextDesignFactoryBase<SubscriptionsDbContext>
{
    public CustomerDbContextDesignFactory()
        : base("PostgresOptions:ConnectionString") { }
}
