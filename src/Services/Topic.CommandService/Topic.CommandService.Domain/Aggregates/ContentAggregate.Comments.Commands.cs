using Core.Events.Comments.CreateComment;
using Core.Events.Comments.RemoveComment;
using Core.Events.Comments.UpdateComment;
using Marten.Events;

namespace Topic.CommandService.Domain.Aggregates;

public partial class ContentAggregate
{
    private readonly Dictionary<Guid, (string text, string authorName)> comments = [];

    public void CreateComment(string commentText, string authorName)
    {
        EnsureTopicIsActive();
        EnsureCommentTextIsValid(commentText);
        
        RegisterEvent(new CreateCommentEvent
        {
            Id = Id,
            CommentId = Guid.NewGuid(),
            Text = commentText,
            AuthorName = authorName,
            CreateDate = DateTime.UtcNow,
        });
    }

    public void Apply(CreateCommentEvent @event)
    {
        Id = @event.Id;
        comments.Add(
            @event.CommentId,
            (@event.Text, @event.AuthorName)
        );
    }

    public void UpdateComment(Guid commentId, string commentText, string authorName)
    {
        EnsureTopicIsActive();
        EnsureMessageIsValid(commentText);
        EnsureCommentBelongsToUser(commentId, authorName);
        
        RegisterEvent(new UpdateCommentEvent
        {
            Id = Id,
            CommentId = commentId,
            Text = commentText,
            AuthorName = authorName,
            UpdateDate = DateTime.UtcNow,
        });
    }

    public void Apply(UpdateCommentEvent @event)
    {
        Id = @event.Id;

        comments[@event.CommentId] = (
            @event.Text,
            @event.AuthorName
        );
    }

    public void RemoveComment(Guid commentId, string username)
    {
        EnsureTopicIsActive();
        EnsureCommentBelongsToUser(commentId, username);
        
        RegisterEvent(new RemoveCommentEvent
        {
            Id = Id,
            CommentId = commentId,
        });
    }

    public void Apply(RemoveCommentEvent @event)
    {
        Id = @event.Id;
        comments.Remove(@event.CommentId);
    }
}