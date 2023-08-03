namespace Ytsoob.Services.Posts.Polls.Dtos;

public class VoterDto
{
    public long Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Username { get; set; }
    public string? Avatar { get; set; }
}
