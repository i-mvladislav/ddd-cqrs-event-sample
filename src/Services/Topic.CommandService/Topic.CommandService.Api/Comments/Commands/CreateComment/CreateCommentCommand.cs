namespace Topic.CommandService.Api.Comments.Commands.CreateComment;

public class CreateCommentCommand: BaseCommand
{
    public string Text { get; set; } = null!;
    public string AuthorName { get; set; } = null!;
}