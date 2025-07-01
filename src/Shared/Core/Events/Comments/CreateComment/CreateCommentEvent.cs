namespace Core.Events.Comments.CreateComment;

public class CreateCommentEvent: BaseEvent
{
    public required Guid CommentId { get; set; }
    public string Text { get; set; } = null!;
    public string AuthorName { get; set; } = null!;
    public DateTime CreateDate { get; set; }

    public CreateCommentEvent(): base(nameof(CreateCommentEvent))
    {
        
    }
}