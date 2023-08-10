using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Core.CQRS.Events.Internal;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Subscriptions.Shared.Contracts;
using Ytsoob.Services.Subscriptions.Subscriptions.Exceptions;
using Ytsoob.Services.Subscriptions.Subscriptions.Models;
using Ytsoob.Services.Subscriptions.Subscriptions.ValueObjects;

namespace Ytsoob.Services.Subscriptions.Subscriptions.Features.RemovingSubscription.v1.RemoveSubscription;

public record SubscriptionRemovedDomainEvent(long Id) : DomainEvent;

public record RemoveSubscription(long Id) : ITxCommand;

public class RemoveSubscriptionEndpoint : ICommandMinimalEndpoint<RemoveSubscription>
{
    public async Task<IResult> HandleAsync(
        HttpContext context,
        RemoveSubscription request,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(request, nameof(request));

        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(RemoveSubscriptionEndpoint)))
        using (Serilog.Context.LogContext.PushProperty(nameof(SubscriptionId), request.Id))
        {
            var result = await commandProcessor.SendAsync(request, cancellationToken);

            return Results.Ok(result);
        }
    }

    public string GroupName => SubscriptionsConfigs.Tag;
    public string PrefixRoute => SubscriptionsConfigs.PrefixUri;
    public double Version => 1.0;

    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapDelete(nameof(RemoveSubscription), HandleAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName(nameof(RemoveSubscription))
            .WithDisplayName(nameof(RemoveSubscription).Pluralize());
    }
}

public class RemoveSubscriptionHandler : ICommandHandler<RemoveSubscription>
{
    private ISubscriptionsDbContext _subscriptionsDbContext;
    private ICurrentUserService _currentUserService;

    public RemoveSubscriptionHandler(
        ICurrentUserService currentUserService,
        ISubscriptionsDbContext subscriptionsDbContext
    )
    {
        _currentUserService = currentUserService;
        _subscriptionsDbContext = subscriptionsDbContext;
    }

    public async Task<Unit> Handle(RemoveSubscription request, CancellationToken cancellationToken)
    {
        Subscription? subscription = await _subscriptionsDbContext.Subscriptions.FirstOrDefaultAsync(
            x => x.Id == request.Id && x.CreatedBy == _currentUserService.YtsooberId,
            cancellationToken: cancellationToken
        );
        if (subscription == null)
            throw new SubscriptionNotFoundException(request.Id);
        subscription.Remove();
        _subscriptionsDbContext.Subscriptions.Remove(subscription);
        await _subscriptionsDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
