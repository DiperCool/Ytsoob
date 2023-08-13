using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Core.CQRS.Events.Internal;
using BuildingBlocks.Security.Jwt;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Subscriptions.Shared.Contracts;
using Ytsoob.Services.Subscriptions.Subscriptions.Exceptions;
using Ytsoob.Services.Subscriptions.Subscriptions.Models;
using Ytsoob.Services.Subscriptions.Subscriptions.ValueObjects;

namespace Ytsoob.Services.Subscriptions.Subscriptions.Features.UpdatingSubscription.v1.UpdateSubscription;

public record SubscriptionUpdatedDomainEvent(long Id, string Title, string Description, string? Photo, decimal Price)
    : DomainEvent;

public record UpdateSubscription(long Id, string Title, string Description, decimal Price) : ITxUpdateCommand;

public class UpdateSubscriptionValidator : AbstractValidator<UpdateSubscription>
{
    public UpdateSubscriptionValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Price).GreaterThan(0);
    }
}

public class UpdateSubscriptionEndpoint : ICommandMinimalEndpoint<UpdateSubscription>
{
    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromBody] UpdateSubscription request,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(request, nameof(request));

        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(UpdateSubscriptionEndpoint)))
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
            .MapPut(nameof(UpdatingSubscription), HandleAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName(nameof(UpdatingSubscription))
            .WithDisplayName(nameof(UpdatingSubscription).Pluralize());
    }
}

public class UpdateSubscriptionHandler : ICommandHandler<UpdateSubscription>
{
    private ICurrentUserService _currentUserService;
    private ISubscriptionsDbContext _subscriptionsDbContext;

    public UpdateSubscriptionHandler(
        ISubscriptionsDbContext subscriptionsDbContext,
        ICurrentUserService currentUserService
    )
    {
        _subscriptionsDbContext = subscriptionsDbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(UpdateSubscription request, CancellationToken cancellationToken)
    {
        Subscription? subscription = await _subscriptionsDbContext.Subscriptions.FirstOrDefaultAsync(
            x => x.Id == request.Id && x.CreatedBy == _currentUserService.YtsooberId,
            cancellationToken: cancellationToken
        );
        if (subscription == null)
            throw new SubscriptionNotFoundException(request.Id);
        subscription.Update(
            Title.Of(request.Title),
            Description.Of(request.Description),
            subscription.Photo,
            Price.Of(request.Price)
        );
        _subscriptionsDbContext.Subscriptions.Update(subscription);
        await _subscriptionsDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
