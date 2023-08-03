using BuildingBlocks.Core.Domain;
using Ytsoob.Services.Posts.Contents.Models;
using Ytsoob.Services.Posts.Contents.ValueObjects;
using Ytsoob.Services.Posts.Polls.Feature.Voting.v1.Vote;
using Ytsoob.Services.Posts.Polls.Feature.Voting.v1.Vote.Events;
using Ytsoob.Services.Posts.Polls.Models;
using Ytsoob.Services.Posts.Posts.Exception;
using Ytsoob.Services.Posts.Posts.Features.CreatingPost.v1.Events;
using Ytsoob.Services.Posts.Posts.Features.DeletingPost;
using Ytsoob.Services.Posts.Posts.Features.UpdatingTextPost.v1.Events;
using Ytsoob.Services.Posts.Posts.ValueObjects;
using Ytsoob.Services.Posts.Users.Features.Models;

namespace Ytsoob.Services.Posts.Posts.Models;

public class Post : Aggregate<PostId>
{
    public Content Content { get; private set; } = default!;
    public Poll? Poll { get; private set; }

    public Post(PostId postId, Content content, Poll? poll)
    {
        Id = postId;
        Content = content;
        Poll = poll;
    }

    // ef
    protected Post() { }

    public void UpdateContentText(ContentText contentText)
    {
        Content.UpdateText(contentText);
        AddDomainEvents(new PostContentUpdated(Id, contentText, Content.Files));
    }

    public static Post Create(PostId postId, Content content, Poll? poll)
    {
        Post post = new Post(postId, content, poll);
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
}
