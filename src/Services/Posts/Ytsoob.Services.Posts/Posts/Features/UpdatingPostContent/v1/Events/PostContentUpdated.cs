using BuildingBlocks.Core.CQRS.Events.Internal;
using Ytsoob.Services.Posts.Contents.ValueObjects;
using Ytsoob.Services.Posts.Posts.ValueObjects;

namespace Ytsoob.Services.Posts.Posts.Features.UpdatingTextPost.v1.Events;

public record PostContentUpdated(PostId PostId, ContentText ContentText) : DomainEvent;
