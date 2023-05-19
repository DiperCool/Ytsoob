using AutoMapper;
using Ytsoob.Services.Posts.Contents.Dtos;
using Ytsoob.Services.Posts.Contents.Models;
using Ytsoob.Services.Posts.Posts.Dtos;
using Ytsoob.Services.Posts.Posts.Features.CreatingPost.v1;
using Ytsoob.Services.Posts.Posts.Models;

namespace Ytsoob.Services.Posts.Posts;

public class PostMapper : Profile
{
    public PostMapper()
    {
        CreateMap<Post, PostDto>();
        CreateMap<Content, ContentDto>();
        CreateMap<CreatePostRequest, CreatePost>().ConstructUsing(
            req =>
                new CreatePost(
                    req.Content
                )
        );
    }
}
