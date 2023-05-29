using BuildingBlocks.Core.CQRS.Events.Internal;
using Ytsoob.Services.Posts.Posts.Models;

namespace Ytsoob.Services.Posts.Posts.Features.DeletingPost;

public record PostDeleted(Post Post) : DomainEvent;
