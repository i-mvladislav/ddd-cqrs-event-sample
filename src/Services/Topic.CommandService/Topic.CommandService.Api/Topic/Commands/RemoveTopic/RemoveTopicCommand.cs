namespace Topic.CommandService.Api.Topic.Commands.RemoveTopic;

public class RemoveTopicCommand: BaseCommand
{
    public string AuthorName { get; set; } = null!;
}