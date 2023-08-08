using AutoMapper;
using Ytsoob.Services.Posts.Comments.Models;
using Ytsoob.Services.Posts.Reactions;

namespace Ytsoob.Services.Posts.Comments;

public class CommentsMapper : Profile
{
    public CommentsMapper()
    {
        CreateMap<BaseComment, CommentDto>()
            .ForMember(x => x.Id, expression => expression.MapFrom(x => x.Id.Value))
            .ForMember(x => x.Content, expression => expression.MapFrom(x => x.Content.Value))
            .ForMember(x => x.PostId, expression => expression.MapFrom(x => x.PostId.Value));
        this.CreateMapReactionStats();
    }
}
