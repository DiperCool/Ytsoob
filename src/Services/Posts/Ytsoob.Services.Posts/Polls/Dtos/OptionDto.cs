namespace Ytsoob.Services.Posts.Polls.Dtos;

public class OptionDto
{
    public long Id { get; set; }
    public string Title { get; set; } = default!;
    public long Count { get; set; }
    public long Fiction { get; set; }
}
