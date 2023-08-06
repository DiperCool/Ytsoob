using BuildingBlocks.Core.Domain;
using Ytsoob.Services.Posts.Contents.Models;
using Ytsoob.Services.Posts.Contents.ValueObjects;
using Ytsoob.Services.Posts.Polls.Feature.Unvoting.v1.Events;
using Ytsoob.Services.Posts.Polls.Feature.Voting.v1.Vote.Events;
using Ytsoob.Services.Posts.Polls.Models;
using Ytsoob.Services.Posts.Posts.Exception;
using Ytsoob.Services.Posts.Posts.Features.CreatingPost.v1.Events;
using Ytsoob.Services.Posts.Posts.Features.DeletingPost;
using Ytsoob.Services.Posts.Posts.Features.UpdatingTextPost.v1.Events;
using Ytsoob.Services.Posts.Posts.ValueObjects;
using Ytsoob.Services.Posts.Reactions.Enums;
using Ytsoob.Services.Posts.Reactions.Models;
using Ytsoob.Services.Posts.Shared.Contracts;
using Ytsoob.Services.Posts.Users.Features.Models;

namespace Ytsoob.Services.Posts.Posts.Models;

public class Post : Aggregate<PostId>, IEntityWithReactions<PostId>
{
    public Content Content { get; private set; } = default!;
    public Poll? Poll { get; private set; }
    public ReactionStats ReactionStats { get; private set; }

    public Post(PostId postId, Content content, Poll? poll, ReactionStats reactionStats)
    {
        Id = postId;
        Content = content;
        Poll = poll;
        ReactionStats = reactionStats;
    }

    // ef
    protected Post() { }

    public void UpdateContentText(ContentText contentText)
    {
        Content.UpdateText(contentText);
        AddDomainEvents(new PostContentUpdated(Id, contentText, Content.Files));
    }

    public static Post Create(PostId postId, Content content, Poll? poll, ReactionStats reactionStats)
    {
        Post post = new Post(postId, content, poll, reactionStats);
        post.AddDomainEvents(new PostCreated(post));
        return post;
    }

    public void Delete()
    {
        AddDomainEvents(new PostDeleted(this));
    }

    public void AddFileToContent(string fileUrl)
    {
        Content.AddFile(fileUrl);
        AddDomainEvents(new PostContentUpdated(Id, Content.ContentText, Content.Files));
    }

    public void RemoveFileFromContent(string fileUrl)
    {
        Content.RemoveFile(fileUrl);
        AddDomainEvents(new PostContentUpdated(Id, Content.ContentText, Content.Files));
    }

    public void VotePoll(Ytsoober voter, Option option)
    {
        if (Poll == null)
            throw new PollIsEmptyException(Id);
        Poll.Vote(voter, option);
        AddDomainEvents(new VoterVotedDomainEvent(Poll, option.Id, voter.Id));
    }

    public void UnvotePoll(Ytsoober voter, Option option)
    {
        if (Poll == null)
            throw new PollIsEmptyException(Id);
        Poll.Unvote(voter, option);
        AddDomainEvents(new VoterUnvotedDomainEvent(Poll, option.Id, voter.Id));
    }

    public void AddReaction(ReactionType reactionType)
    {
        ReactionStats.AddReaction(reactionType);
    }

    public void RemoveReaction(ReactionType reactionType)
    {
        ReactionStats.RemoveReaction(reactionType);
    }
}
