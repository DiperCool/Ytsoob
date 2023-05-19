using BuildingBlocks.Persistence.EfCore.Postgres;

namespace Ytsoob.Services.Posts.Shared.Data;

public class CustomerDbContextDesignFactory : DbContextDesignFactoryBase<PostsDbContext>
{
    public CustomerDbContextDesignFactory()
        : base("PostgresOptions:ConnectionString") { }
}
