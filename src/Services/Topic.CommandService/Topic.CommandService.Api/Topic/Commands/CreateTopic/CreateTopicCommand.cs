namespace Topic.CommandService.Api.Topic.Commands.CreateTopic;

public class CreateTopicCommand: BaseCommand
{
    public string AuthorName { get; set; } = null!;
    public string MessageText { get; set; } = null!;
}