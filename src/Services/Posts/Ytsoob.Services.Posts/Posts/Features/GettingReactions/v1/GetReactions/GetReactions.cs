using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Queries;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Core.CQRS.Queries;
using BuildingBlocks.Core.Persistence.EfCore;
using BuildingBlocks.Security.Jwt;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Posts.Posts.ValueObjects;
using Ytsoob.Services.Posts.Reactions.Dtos;
using Ytsoob.Services.Posts.Reactions.Models;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Posts.Features.GettingReactions.v1.GetReactions;

public record GetReactionsResponse(ListResultModel<ReactionDto> Reactions);

public record GetReactions(long PostId) : ListQuery<GetReactionsResponse>;

public class GetPostsValidator : AbstractValidator<GetReactions>
{
    public GetPostsValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Page).GreaterThanOrEqualTo(1).WithMessage("Page should at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageSize should at least greater than or equal to 1.");
    }
}

public class GetPostsEndpoint : IMinimalEndpoint
{
    public string GroupName => PostsConfigs.Tag;
    public string PrefixRoute => PostsConfigs.PostPrefixUri;
    public double Version => 1.0;

    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapGet("/GetReactions", HandleAsync)
            .Produces<GetReactionsResponse>(StatusCodes.Status200OK)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName("GetReactions")
            .WithDisplayName("Get Reactions.");
    }

    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromQuery] long postId,
        [FromQuery] int page,
        [FromQuery] int pageSize,
        IQueryProcessor queryProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        GetReactionsResponse result = await queryProcessor.SendAsync(
            new GetReactions(postId) { Page = page, PageSize = pageSize },
            cancellationToken
        );

        return Results.Ok(result);
    }
}

public class GetReactionsHandler : IQueryHandler<GetReactions, GetReactionsResponse>
{
    private IPostsDbContext _postsDbContext;
    private IMapper _mapper;
    private ICurrentUserService _currentUserService;

    public GetReactionsHandler(IPostsDbContext postsDbContext, IMapper mapper, ICurrentUserService currentUserService)
    {
        _postsDbContext = postsDbContext;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<GetReactionsResponse> Handle(GetReactions request, CancellationToken cancellationToken)
    {
        PostId postId = PostId.Of(request.PostId);
        var reactions = await _postsDbContext.Reactions
            .Where(x => x.EntityId == postId.ToString())
            .Include(x => x.Ytsoober)
            .AsNoTracking()
            .ApplyPagingAsync<Reaction, ReactionDto>(
                _mapper.ConfigurationProvider,
                request.Page,
                request.PageSize,
                cancellationToken: cancellationToken
            );

        return new GetReactionsResponse(reactions);
    }
}
