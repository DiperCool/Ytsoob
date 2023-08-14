using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Core.Exception.Types;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Subscriptions.Shared.Contracts;
using Ytsoob.Services.Subscriptions.Subscriptions.Contracts;
using Ytsoob.Services.Subscriptions.Subscriptions.Exceptions;
using Ytsoob.Services.Subscriptions.Subscriptions.Models;
using Ytsoob.Services.Subscriptions.Subscriptions.ValueObjects;

namespace Ytsoob.Services.Subscriptions.Subscriptions.Features.RemovingPhoto.v1.RemovePhoto;

public record RemovePhoto(long SubId) : ITxUpdateCommand;

public class RemovePhotoEndpoint : ICommandMinimalEndpoint<RemovePhoto>
{
    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromBody] RemovePhoto request,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(request, nameof(request));

        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(RemovePhotoEndpoint)))
        using (Serilog.Context.LogContext.PushProperty(nameof(SubscriptionId), request.SubId))
        {
            var result = await commandProcessor.SendAsync(request, cancellationToken);

            return Results.NoContent();
        }
    }

    public string GroupName => SubscriptionsConfigs.Tag;
    public string PrefixRoute => SubscriptionsConfigs.PrefixUri;
    public double Version => 1.0;

    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapDelete(nameof(RemovePhoto), HandleAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName(nameof(RemovePhoto))
            .WithDisplayName(nameof(RemovePhoto).Pluralize());
    }
}

public class RemovePhotoHandler : ICommandHandler<RemovePhoto>
{
    private ISubBlobStorage _subBlobStorage;
    private ICurrentUserService _currentUserService;
    private ISubscriptionsDbContext _subscriptionsDbContext;

    public RemovePhotoHandler(
        ISubBlobStorage subBlobStorage,
        ICurrentUserService currentUserService,
        ISubscriptionsDbContext subscriptionsDbContext
    )
    {
        _subBlobStorage = subBlobStorage;
        _currentUserService = currentUserService;
        _subscriptionsDbContext = subscriptionsDbContext;
    }

    public async Task<Unit> Handle(RemovePhoto request, CancellationToken cancellationToken)
    {
        Subscription? subscription = await _subscriptionsDbContext.Subscriptions.FirstOrDefaultAsync(
            x => x.Id == request.SubId && x.CreatedBy == _currentUserService.YtsooberId,
            cancellationToken: cancellationToken
        );
        if (subscription == null)
            throw new SubscriptionNotFoundException(request.SubId);
        if (string.IsNullOrEmpty(subscription.Photo))
            throw new BadRequestException("Photo is not exists");
        await _subBlobStorage.RemoveFileAsync(subscription.Photo, cancellationToken);
        subscription.Update(subscription.Title, subscription.Description, null, subscription.Price);
        _subscriptionsDbContext.Subscriptions.Update(subscription);
        await _subscriptionsDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
