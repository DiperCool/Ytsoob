using Ytsoob.Services.Posts.Comments.ValueObjects;

namespace Ytsoob.Services.Posts.Comments.Models;

public class RepliedComment : BaseComment
{
    public CommentId CommentId { get; set; } = default!;
    public Comment Comment { get; set; } = default!;
}
