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
