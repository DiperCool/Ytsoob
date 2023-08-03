using BuildingBlocks.Core.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Posts.Contents.Models;
using Ytsoob.Services.Posts.Polls.Models;
using Ytsoob.Services.Posts.Posts.Models;
using Ytsoob.Services.Posts.Shared.Contracts;
using Ytsoob.Services.Posts.Users.Features.Models;

namespace Ytsoob.Services.Posts.Shared.Data;

public class PostsDbContext : EfDbContextBase, IPostsDbContext
{
    public const string DefaultSchema = "posts";

    public PostsDbContext(DbContextOptions<PostsDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension(EfConstants.UuidGenerator);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Ytsoober> Ytsoobers => Set<Ytsoober>();
    public DbSet<Content> Contents => Set<Content>();
    public DbSet<Poll> Polls => Set<Poll>();
    public DbSet<Option> Options => Set<Option>();
    public DbSet<Voter> Voters => Set<Voter>();
}
