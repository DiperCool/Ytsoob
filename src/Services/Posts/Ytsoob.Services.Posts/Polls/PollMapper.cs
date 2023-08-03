using AutoMapper;
using Ytsoob.Services.Posts.Polls.Dtos;
using Ytsoob.Services.Posts.Polls.Models;
using Ytsoob.Services.Posts.Users.Features.Models;

namespace Ytsoob.Services.Posts.Polls;

public class PollMapper : Profile
{
    public PollMapper()
    {
        CreateMap<Poll, PollDto>().ForMember(x => x.Id, expression => expression.MapFrom(poll => poll.Id.Value));
        CreateMap<Option, OptionDto>()
            .ForMember(x => x.Id, expression => expression.MapFrom(poll => poll.Id.Value))
            .ForMember(x => x.Count, expression => expression.MapFrom(poll => poll.Count.Value))
            .ForMember(x => x.Fiction, expression => expression.MapFrom(poll => poll.Fiction.Value))
            .ForMember(x => x.Title, expression => expression.MapFrom(poll => poll.Title.Value));
        CreateMap<Ytsoober, Voter>();
    }
}
