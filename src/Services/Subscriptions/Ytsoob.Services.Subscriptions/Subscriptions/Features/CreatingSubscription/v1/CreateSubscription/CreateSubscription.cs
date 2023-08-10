using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Core.CQRS.Events.Internal;
using BuildingBlocks.Core.IdsGenerator;
using BuildingBlocks.Security.Jwt;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Humanizer;
using Ytsoob.Services.Subscriptions.Shared.Contracts;
using Ytsoob.Services.Subscriptions.Subscriptions.Dtos;
using Ytsoob.Services.Subscriptions.Subscriptions.Models;
using Ytsoob.Services.Subscriptions.Subscriptions.ValueObjects;

namespace Ytsoob.Services.Subscriptions.Subscriptions.Features.CreatingSubscription.v1.CreateSubscription;

public record SubscriptionCreatedDomainEvent(long Id, string Title, string Description, string? Photo, decimal Price)
    : DomainEvent;

public record CreateSubscription(string Title, string Description, decimal Price) : ITxCommand<SubscriptionDto>
{
    public long Id { get; init; } = SnowFlakIdGenerator.NewId();
}

public class CreateSubscriptionValidator : AbstractValidator<CreateSubscription>
{
    public CreateSubscriptionValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Price).GreaterThan(0);

        RuleFor(x => x.Title).NotEmpty().MaximumLength(50);
    }
}

public class CreateSubscriptionEndpoint : ICommandMinimalEndpoint<CreateSubscription>
{
    public async Task<IResult> HandleAsync(
        HttpContext context,
        CreateSubscription request,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(request, nameof(request));

        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(CreateSubscriptionEndpoint)))
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
            .MapPost(nameof(CreateSubscription), HandleAsync)
            .RequireAuthorization()
            .Produces<SubscriptionDto>()
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName(nameof(CreateSubscription))
            .WithDisplayName(nameof(CreateSubscription).Pluralize());
    }
}

public class CreateSubscriptionHandler : ICommandHandler<CreateSubscription, SubscriptionDto>
{
    private ISubscriptionsDbContext _subscriptionsDbContext;
    private ICurrentUserService _currentUserService;
    private IMapper _mapper;

    public CreateSubscriptionHandler(
        ISubscriptionsDbContext subscriptionsDbContext,
        ICurrentUserService currentUserService,
        IMapper mapper
    )
    {
        _subscriptionsDbContext = subscriptionsDbContext;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<SubscriptionDto> Handle(CreateSubscription request, CancellationToken cancellationToken)
    {
        Subscription subscription = Subscription.Create(
            SubscriptionId.Of(request.Id),
            Title.Of(request.Title),
            Description.Of(request.Description),
            Price.Of(request.Price),
            _currentUserService.YtsooberId
        );
        await _subscriptionsDbContext.Subscriptions.AddAsync(subscription, cancellationToken);
        await _subscriptionsDbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<SubscriptionDto>(subscription);
    }
}
