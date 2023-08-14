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

namespace Ytsoob.Services.Subscriptions.Subscriptions.Features.AddingPhoto.v1.AddPhoto;

public record AddPhotoResult(string File);

public record AddPhoto(long SubId, IFormFile File) : ITxUpdateCommand<AddPhotoResult>;

public class AddPhotoEndpoint : IMinimalEndpoint
{
    public string GroupName => SubscriptionsConfigs.Tag;
    public string PrefixRoute => SubscriptionsConfigs.PrefixUri;
    public double Version => 1.0;

    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPost(nameof(AddPhoto), HandleAsync)
            .RequireAuthorization()
            .Produces<AddPhotoResult>()
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName(nameof(AddPhoto))
            .WithDisplayName(nameof(AddPhoto).Pluralize());
    }

    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromQuery] long subId,
        [FromForm] IFormFile file,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(subId, nameof(subId));

        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(AddPhotoEndpoint)))
        using (Serilog.Context.LogContext.PushProperty("SubId", subId))
        {
            var result = await commandProcessor.SendAsync(new AddPhoto(subId, file), cancellationToken);

            return Results.Ok(result);
        }
    }
}

public class AddFilesHandler : ICommandHandler<AddPhoto, AddPhotoResult>
{
    private ISubBlobStorage _subBlobStorage;
    private ISubscriptionsDbContext _subscriptionsDbContext;
    private ICurrentUserService _currentUserService;

    public AddFilesHandler(
        ISubBlobStorage subBlobStorage,
        ISubscriptionsDbContext subscriptionsDbContext,
        ICurrentUserService currentUserService
    )
    {
        _subBlobStorage = subBlobStorage;
        _subscriptionsDbContext = subscriptionsDbContext;
        _currentUserService = currentUserService;
    }

    public async Task<AddPhotoResult> Handle(AddPhoto request, CancellationToken cancellationToken)
    {
        Subscription? subscription = await _subscriptionsDbContext.Subscriptions.FirstOrDefaultAsync(
            x => x.Id == request.SubId && x.CreatedBy == _currentUserService.YtsooberId,
            cancellationToken: cancellationToken
        );
        if (subscription == null)
            throw new SubscriptionNotFoundException(request.SubId);
        string? fileUrl = await _subBlobStorage.UploadFileAsync(request.File, cancellationToken);
        if (string.IsNullOrEmpty(fileUrl))
            throw new BadRequestException("Failed to upload image");
        subscription.Update(subscription.Title, subscription.Description, fileUrl, subscription.Price);
        _subscriptionsDbContext.Subscriptions.Update(subscription);
        await _subscriptionsDbContext.SaveChangesAsync(cancellationToken);
        return new AddPhotoResult(fileUrl);
    }
}
