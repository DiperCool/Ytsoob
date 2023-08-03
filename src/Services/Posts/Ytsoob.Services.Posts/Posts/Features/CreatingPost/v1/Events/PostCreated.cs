using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Core.CQRS.Events.Internal;
using BuildingBlocks.Core.Exception.Types;
using Ytsoob.Services.Posts.Posts.Models;

namespace Ytsoob.Services.Posts.Posts.Features.CreatingPost.v1.Events;

public record PostCreated(Post Post) : DomainEvent;

public class PostCreatedHandler : IDomainEventHandler<PostCreated>
{
    private ILogger<PostCreatedHandler> _logger;

    public PostCreatedHandler(ILogger<PostCreatedHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(PostCreated notification, CancellationToken cancellationToken) { }
}
