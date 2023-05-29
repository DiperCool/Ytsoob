using BuildingBlocks.Core.CQRS.Events.Internal;
using Ytsoob.Services.Posts.Posts.Models;

namespace Ytsoob.Services.Posts.Posts.Features.CreatingPost.v1.Events;

public record PostCreated(Post Post) : DomainEvent;
