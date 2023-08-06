using Ytsoob.Services.Posts.Reactions.Enums;
using Ytsoob.Services.Posts.Users.Dtos;

namespace Ytsoob.Services.Posts.Reactions.Dtos;

public class ReactionDto
{
    public ReactionType ReactionType { get; set; }
    public YtsooberDto Ytsoober { get; set; } = default!;
}
