using Core.Events.Topics.CreateTopic;
using Core.Events.Topics.LikeTopic;
using Core.Events.Topics.RemoveTopic;
using Core.Events.Topics.UpdateTopic;

namespace Topic.CommandService.Domain.Aggregates;

public partial class ContentAggregate
{
    private string author = null!;
    public bool Active { get; set; }

    public ContentAggregate(Guid id, string authorName, string messageText)
    {
        RegisterEvent(new CreateTopicEvent
        {
            Id = id,
            AuthorName = authorName,
            MessageText = messageText,
            CreateDate = DateTime.UtcNow,
        });
    }

    public void Apply(CreateTopicEvent createTopicEvent)
    {
        Active = true;
        Id = createTopicEvent.Id;
        author = createTopicEvent.AuthorName;
    }

    public void UpdateTopic(string messageText)
    {
        EnsureTopicIsActive();
        EnsureMessageIsValid(messageText);
        
        RegisterEvent(new UpdateTopicEvent
        {
            Id = Id,
            MessageText = messageText,
        });
    }

    public void Apply(UpdateTopicEvent updateTopicEvent)
    {
        Id = updateTopicEvent.Id;
    }

    public void RemoveTopic(string username)
    {
        EnsureTopicIsActive();
        EnsureUserIsAuthor(username);
        RegisterEvent(new RemoveTopicEvent { Id = Id });
    }

    public void Apply(RemoveTopicEvent removeTopicEvent)
    {
        Id = removeTopicEvent.Id;
        Active = false;
    }

    public void LikeTopic()
    {
        EnsureTopicIsActive();
        RegisterEvent(new LikeTopicEvent { Id = Id });
    }

    public void Apply(LikeTopicEvent likeTopicEvent)
    {
        Id = likeTopicEvent.Id;
    }
}