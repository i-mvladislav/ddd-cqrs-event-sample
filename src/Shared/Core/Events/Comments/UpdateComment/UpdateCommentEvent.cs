namespace Core.Events.Comments.UpdateComment;

public class UpdateCommentEvent: BaseEvent
{
    public required Guid CommentId { get; set; }
    public string Text { get; set; } = null!;
    public string AuthorName { get; set; } = null!;
    public DateTime UpdateDate { get; set; }

    public UpdateCommentEvent(): base(nameof(UpdateCommentEvent))
    {
        
    }
}