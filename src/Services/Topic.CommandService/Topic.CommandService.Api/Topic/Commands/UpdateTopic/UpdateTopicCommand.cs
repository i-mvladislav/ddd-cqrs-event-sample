namespace Topic.CommandService.Api.Topic.Commands.UpdateTopic;

public class UpdateTopicCommand: BaseCommand
{
    public string MessageText { get; set; } = null!;
}