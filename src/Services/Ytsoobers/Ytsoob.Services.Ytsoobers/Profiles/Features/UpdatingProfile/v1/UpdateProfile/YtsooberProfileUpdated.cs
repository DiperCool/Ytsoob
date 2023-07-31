using BuildingBlocks.Core.CQRS.Events.Internal;
using Ytsoob.Services.Ytsoobers.Profiles.ValueObjects;
using Ytsoob.Services.Ytsoobers.Ytsoobers.ValueObjects;

namespace Ytsoob.Services.Ytsoobers.Ytsoobers.Events;

public record YtsooberProfileUpdated(YtsooberId Id, FirstName FirstName, LastName LastName, string? Avatar) : DomainEvent;
