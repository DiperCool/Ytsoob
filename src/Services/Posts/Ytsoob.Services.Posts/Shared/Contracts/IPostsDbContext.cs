using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Posts.Contents.Models;
using Ytsoob.Services.Posts.Posts.Models;
using Ytsoob.Services.Posts.Users.Features.Models;

namespace Ytsoob.Services.Posts.Shared.Contracts;

public interface IPostsDbContext
{
    DbSet<TEntity> Set<TEntity>()
        where TEntity : class;
    DbSet<Post> Posts { get;  }
    DbSet<User> Users { get;  }
    DbSet<Content> Contents { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
