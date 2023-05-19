using BuildingBlocks.Core.Messaging;

namespace Ytsoob.Services.Shared.Ytsoober.Ytsoobers.Events.v1.Integration;

public record YtsooberCreatedV1(
    Guid Id,
    string? Name,
    string State
) : IntegrationEvent;
