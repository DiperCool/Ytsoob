using System.Globalization;
using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using Hellang.Middleware.ProblemDetails;
using Ytsoob.Services.Posts.Posts.Features.UpdatingTextPost.v1;

namespace Ytsoob.Services.Posts.Posts.Features.CreatingPost.v1;

public static class UpdatePostEndpoint
{
    internal static RouteHandlerBuilder MapUpdatePostEndpoint(this IEndpointRouteBuilder endpoints)
    {
        // https://github.com/dotnet/aspnetcore/issues/45082
        // https://github.com/dotnet/aspnetcore/issues/40753
        // https://github.com/domaindrivendev/Swashbuckle.AspNetCore/pull/2414
        // https://github.com/dotnet/aspnetcore/issues/45871
        return endpoints
            .MapPost("/", CreatePosts)
            // WithOpenApi should placed before versioning and other things - this fixed in Aps.Versioning.Http 7.0.0-preview.1
            .WithOpenApi(operation =>
            {
                // we could use our `WithResponseDescription` extension method also
                operation.Summary = "Creating a New Post";
                operation.Description = "Creating a New Post";
                operation.Responses[
                    StatusCodes.Status401Unauthorized.ToString(CultureInfo.InvariantCulture)
                ].Description = "UnAuthorized request.";
                operation.Responses[
                    StatusCodes.Status400BadRequest.ToString(CultureInfo.InvariantCulture)
                ].Description = "Invalid input for creating post.";
                operation.Responses[StatusCodes.Status201Created.ToString(CultureInfo.InvariantCulture)].Description =
                    "Product created successfully.";

                return operation;
            })
            .RequireAuthorization()
            .Produces<CreatePostResponse>(StatusCodes.Status201Created)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName("CreatePost")
            .WithDisplayName("Create a new post.")
            .MapToApiVersion(1.0);

        // .WithMetadata(new SwaggerResponseAttribute(
        //     StatusCodes.Status401Unauthorized,
        //     "UnAuthorized request.",
        //     typeof(StatusCodeProblemDetails)))
        // .WithMetadata(new SwaggerResponseAttribute(
        //     StatusCodes.Status400BadRequest,
        //     "Invalid input for creating product.",
        //     typeof(StatusCodeProblemDetails)))
        // .WithMetadata(
        //     new SwaggerResponseAttribute(
        //         StatusCodes.Status201Created,
        //         "Product created successfully.",
        //         typeof(CreateProductResponse)))
        // .WithMetadata(new SwaggerOperationAttribute("Creating a New Product", "Creating a New Product"))
        // .IsApiVersionNeutral()
    }

    private static async Task<IResult> CreatePosts(
        UpdatePostContentRequest contentRequest,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(contentRequest, nameof(contentRequest));

        var command = mapper.Map<UpdatePostContent>(contentRequest);
        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(UpdatePostEndpoint)))
        using (Serilog.Context.LogContext.PushProperty("PostId", command.PostId))
        {
            var result = await commandProcessor.SendAsync(command, cancellationToken);

            return Results.Ok(result);
        }
    }
}
