using AutoMapper;
using Ytsoob.Services.Posts.Reactions.Dtos;
using Ytsoob.Services.Posts.Reactions.Models;
using Ytsoob.Services.Posts.Users.Dtos;
using Ytsoob.Services.Posts.Users.Features.Models;

namespace Ytsoob.Services.Posts.Reactions;

public class ReactionsMapper : Profile
{
    public ReactionsMapper()
    {
        CreateMap<Ytsoober, YtsooberDto>();
        CreateMap<Reaction, ReactionDto>();
    }
}

public static class ReactionsMapperExtension
{
    public static void CreateMapReactionStats(this Profile profile)
    {
        profile
            .CreateMap<ReactionStats, ReactionStatsDto>()
            .ForMember(x => x.Like, expression => expression.MapFrom(x => x.Like.Value))
            .ForMember(x => x.Angry, expression => expression.MapFrom(x => x.Angry.Value))
            .ForMember(x => x.Dislike, expression => expression.MapFrom(x => x.Dislike.Value))
            .ForMember(x => x.Crying, expression => expression.MapFrom(x => x.Crying.Value))
            .ForMember(x => x.Happy, expression => expression.MapFrom(x => x.Happy.Value))
            .ForMember(x => x.Wonder, expression => expression.MapFrom(x => x.Wonder.Value));
    }
}
