namespace Ytsoob.Services.Posts.Users.Dtos;

public class YtsooberDto
{
    public long Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Username { get; set; }
    public string? Avatar { get; set; }
}
