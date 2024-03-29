using AutoMapper;
using Ytsoob.Services.Posts.Contents.Dtos;
using Ytsoob.Services.Posts.Contents.Features.UpdatingPostContent.v1;
using Ytsoob.Services.Posts.Contents.Models;
using Ytsoob.Services.Posts.Polls.Dtos;
using Ytsoob.Services.Posts.Posts.Dtos;
using Ytsoob.Services.Posts.Posts.Features.CreatingPost.v1;
using Ytsoob.Services.Posts.Posts.Features.DeletingPost;
using Ytsoob.Services.Posts.Posts.Models;
using Ytsoob.Services.Posts.Reactions;
using Ytsoob.Services.Posts.Users.Features.Models;

namespace Ytsoob.Services.Posts.Posts;

public class PostMapper : Profile
{
    public PostMapper()
    {
        CreateMap<Post, PostDto>().ForMember(x => x.Id, expression => expression.MapFrom(x => x.Id.Value));
        CreateMap<Content, ContentDto>()
            .ForMember(x => x.ContentText, expression => expression.MapFrom(x => x.ContentText.Value));
        CreateMap<CreatePostRequest, CreatePost>().ConstructUsing(req => new CreatePost(req.Content, req.Poll));
        CreateMap<UpdatePostContentRequest, UpdatePostContent>();
        CreateMap<DeletePostRequest, DeletePost>();
        CreateMap<Ytsoober, VoterDto>();
        this.CreateMapReactionStats();
    }
}
