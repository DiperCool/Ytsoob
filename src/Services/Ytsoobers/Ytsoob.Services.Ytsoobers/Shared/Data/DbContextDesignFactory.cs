using BuildingBlocks.Persistence.EfCore.Postgres;

namespace Ytsoob.Services.Ytsoobers.Shared.Data;

public class CustomerDbContextDesignFactory : DbContextDesignFactoryBase<YtsoobersesDbContext>
{
    public CustomerDbContextDesignFactory()
        : base("PostgresOptions:ConnectionString") { }
}
