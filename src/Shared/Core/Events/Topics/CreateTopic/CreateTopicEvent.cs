namespace Core.Events.Topics.CreateTopic;

public class CreateTopicEvent: BaseEvent
{
    public string AuthorName { get; set; } = null!;
    public string MessageText { get; set; } = null!;
    public DateTime CreateDate { get; set; }

    public CreateTopicEvent(): base(nameof(CreateTopicEvent))
    {
        
    }
}