using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Posts.Contents.Models;
using Ytsoob.Services.Posts.Polls.Models;
using Ytsoob.Services.Posts.Posts.Models;
using Ytsoob.Services.Posts.Users.Features.Models;

namespace Ytsoob.Services.Posts.Shared.Contracts;

public interface IPostsDbContext
{
    DbSet<TEntity> Set<TEntity>()
        where TEntity : class;
    DbSet<Post> Posts { get; }
    DbSet<Ytsoober> Ytsoobers { get; }
    DbSet<Content> Contents { get; }
    DbSet<Polls.Models.Poll> Polls { get; }
    DbSet<Option> Options { get; }
    DbSet<Voter> Voters { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
