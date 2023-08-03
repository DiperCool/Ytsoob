namespace Ytsoob.Services.Posts.Polls.Dtos;

public class PollDto
{
    public long Id { get; set; }
    public string PollAnswerType { get; set; } = null!;
    public IList<OptionDto> Options { get; set; } = new List<OptionDto>();
}
