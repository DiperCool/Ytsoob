using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.CQRS.Queries;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Core.CQRS.Queries;
using BuildingBlocks.Core.Persistence.EfCore;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Posts.Polls;
using Ytsoob.Services.Posts.Posts.Dtos;
using Ytsoob.Services.Posts.Posts.Models;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Posts.Features.GettingPosts.v1.GetPosts;

public record GetPostsResponse(ListResultModel<PostDto> Posts);

public record GetPosts(long YtsooberId) : ListQuery<GetPostsResponse>;

public class GetPostsEndpoint : IMinimalEndpoint
{
    public string GroupName => PostsConfigs.Tag;
    public string PrefixRoute => PostsConfigs.PostPrefixUri;
    public double Version => 1.0;

    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapGet("/get", HandleAsync)
            .Produces<GetPostsResponse>(StatusCodes.Status200OK)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName("GetPosts")
            .WithDisplayName("Get Posts.");
    }

    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromQuery] long ytsooberId,
        [FromQuery] int page,
        [FromQuery] int pageSize,
        IQueryProcessor queryProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        GetPostsResponse result = await queryProcessor.SendAsync(
            new GetPosts(ytsooberId) { Page = page, PageSize = pageSize },
            cancellationToken
        );

        return Results.Ok(result);
    }
}

public class GetPostsValidator : AbstractValidator<GetPosts>
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

public class GetPostsHandler : IRequestHandler<GetPosts, GetPostsResponse>
{
    private IPostsDbContext _postsDbContext;
    private IMapper _mapper;

    public GetPostsHandler(IPostsDbContext postsDbContext, IMapper mapper)
    {
        _postsDbContext = postsDbContext;
        _mapper = mapper;
    }

    public async Task<GetPostsResponse> Handle(GetPosts request, CancellationToken cancellationToken)
    {
        var products = await _postsDbContext.Posts
            .Include(x => x.Poll)
            .ThenInclude(x => x!.Options)
            .Include(x => x.Content)
            .Where(x => x.CreatedBy == request.YtsooberId)
            .OrderByDescending(x => x.Created)
            .AsNoTracking()
            .ApplyPagingAsync<Post, PostDto>(
                _mapper.ConfigurationProvider,
                request.Page,
                request.PageSize,
                cancellationToken: cancellationToken
            );

        return new GetPostsResponse(products);
    }
}
