using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Queries;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Core.CQRS.Queries;
using BuildingBlocks.Core.Persistence.EfCore;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Posts.Polls.Dtos;
using Ytsoob.Services.Posts.Shared.Contracts;
using Ytsoob.Services.Posts.Users.Features.Models;

namespace Ytsoob.Services.Posts.Polls.Feature.GettingVoters.v1.GetVoter;

public record GetVotersResponse(ListResultModel<VoterDto> Voters);

public record GetVoters(long OptionId) : ListQuery<GetVotersResponse>;

public class GetPostsValidator : AbstractValidator<GetVoters>
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

public class GetVotersEndpoint : IMinimalEndpoint
{
    public string GroupName => PollConfigs.Tag;
    public string PrefixRoute => PollConfigs.PollPrefixUri;
    public double Version => 1.0;

    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapGet("/voters", HandleAsync)
            .Produces<GetVotersResponse>()
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName("GetVoters")
            .WithDisplayName("Get Voters.");
    }

    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromQuery] long optionId,
        [FromQuery] int page,
        [FromQuery] int pageSize,
        IQueryProcessor queryProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        GetVotersResponse result = await queryProcessor.SendAsync(
            new GetVoters(optionId) { Page = page, PageSize = pageSize },
            cancellationToken
        );

        return Results.Ok(result);
    }
}

public class GetVotersHandler : IQueryHandler<GetVoters, GetVotersResponse>
{
    private IPostsDbContext _postsDbContext;
    private IMapper _mapper;

    public GetVotersHandler(IPostsDbContext postsDbContext, IMapper mapper)
    {
        _postsDbContext = postsDbContext;
        _mapper = mapper;
    }

    public async Task<GetVotersResponse> Handle(GetVoters request, CancellationToken cancellationToken)
    {
        var voters = await _postsDbContext.Voters
            .AsNoTracking()
            .Where(x => x.OptionId == request.OptionId)
            .OrderByDescending(x => x.Created)
            .Select(x => x.Ytsoober)
            .ApplyPagingAsync<Ytsoober, VoterDto>(
                _mapper.ConfigurationProvider,
                request.Page,
                request.PageSize,
                cancellationToken: cancellationToken
            );

        return new GetVotersResponse(voters);
    }
}
