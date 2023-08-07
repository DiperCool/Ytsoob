using BuildingBlocks.Abstractions.Domain;

namespace Ytsoob.Services.Posts.Comments.ValueObjects;

public record CommentId : AggregateId
{
    protected CommentId(long value)
        : base(value) { }

    public static CommentId Of(long value) => new(value);
}
