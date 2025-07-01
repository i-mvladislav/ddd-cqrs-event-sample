namespace Core.Events.Topics.UpdateTopic;

public class UpdateTopicEvent: BaseEvent
{
    public string MessageText { get; set; } = null!;

    public UpdateTopicEvent(): base(nameof(UpdateTopicEvent))
    {
        
    }
}