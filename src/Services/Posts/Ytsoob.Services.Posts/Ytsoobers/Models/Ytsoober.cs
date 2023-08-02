using Ytsoob.Services.Posts.Poll.Models;

namespace Ytsoob.Services.Posts.Users.Features.Models;

public class Ytsoober
{
    public long Id { get; set; }
    public Guid IdentityId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Username { get; set; }
    public IList<Voter> Voting { get; set; } = new List<Voter>();
}
