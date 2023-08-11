using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Posts.Comments.Models;
using Ytsoob.Services.Posts.Contents.Models;
using Ytsoob.Services.Posts.Polls.Models;
using Ytsoob.Services.Posts.Posts.Models;
using Ytsoob.Services.Posts.Reactions.Models;
using Ytsoob.Services.Posts.Users.Features.Models;

namespace Ytsoob.Services.Posts.Shared.Contracts;

public interface IPostsDbContext
{
    DbSet<TEntity> Set<TEntity>()
        where TEntity : class;
    DbSet<Post> Posts { get; }
    DbSet<Ytsoober> Ytsoobers { get; }
    DbSet<Content> Contents { get; }
    DbSet<Poll> Polls { get; }
    DbSet<Option> Options { get; }
    DbSet<Voter> Voters { get; }
    DbSet<Reaction> Reactions { get; }
    DbSet<BaseComment> BaseComments { get; }
    DbSet<Comment> Comments { get; }
    DbSet<RepliedComment> RepliedComments { get; }
    DbSet<Subscription.Models.Subscription> Subscriptions { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
