using BuildingBlocks.Core.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Ytsoobers.Profiles.Models;
using Ytsoob.Services.Ytsoobers.Shared.Contracts;
using Ytsoob.Services.Ytsoobers.Ytsoobers.Models;

namespace Ytsoob.Services.Ytsoobers.Shared.Data;

public class YtsoobersesDbContext : EfDbContextBase, IYtsoobersDbContext
{
    public const string DefaultSchema = "ytsoober";

    public YtsoobersesDbContext(DbContextOptions<YtsoobersesDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension(EfConstants.UuidGenerator);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Ytsoober> Ytsoobers => Set<Ytsoober>();
    public DbSet<Profile> Profiles => Set<Profile>();

}
