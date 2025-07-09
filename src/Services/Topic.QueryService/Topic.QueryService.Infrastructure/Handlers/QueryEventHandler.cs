using Core.Events.Comments.CreateComment;
using Core.Events.Comments.RemoveComment;
using Core.Events.Comments.UpdateComment;
using Core.Events.Topics.CreateTopic;
using Core.Events.Topics.LikeTopic;
using Core.Events.Topics.RemoveTopic;
using Core.Events.Topics.UpdateTopic;
using Topic.QueryService.Domain.Dao;
using Topic.QueryService.Domain.Entities;

namespace Topic.QueryService.Infrastructure.Handlers;

public class QueryEventHandler(ITopicStorage topicStorage,
    ICommentStorage commentStorage): IQueryEventHandler
{
    public async Task On(CreateTopicEvent createTopicEvent)
    {
        var topic = new TopicEntity
        {
            Id = createTopicEvent.Id,
            AuthorName = createTopicEvent.AuthorName,
            CreatedAt = createTopicEvent.CreateDate,
            MessageText = createTopicEvent.MessageText,
        };

        await topicStorage.AddTopicAsync(topic);
    }

    public async Task On(UpdateTopicEvent updateTopicEvent)
    {
        var topic = await topicStorage.GetTopicByIdAsync(updateTopicEvent.Id);
        if (topic is null) return;
        topic.MessageText = updateTopicEvent.MessageText;
        await topicStorage.UpdateTopicAsync(topic);
    }

    public async Task On(LikeTopicEvent likeTopicEvent)
    {
        var topic = await topicStorage.GetTopicByIdAsync(likeTopicEvent.Id);
        if (topic is null) return;
        topic.Likes++;
        await topicStorage.UpdateTopicAsync(topic);
    }

    public async Task On(RemoveTopicEvent removeTopicEvent)
    {
        await topicStorage.DeleteTopicAsync(removeTopicEvent.Id);
    }

    public async Task On(CreateCommentEvent createCommentEvent)
    {
        var comment = new CommentEntity
        {
            Id = createCommentEvent.CommentId,
            TopicId = createCommentEvent.Id,
            CreatedAt = createCommentEvent.CreateDate,
            MessageText = createCommentEvent.Text,
            AuthorName = createCommentEvent.AuthorName,
            IsTextEdited = false,
        };
        
        await commentStorage.AddCommentAsync(comment);
    }

    public async Task On(UpdateCommentEvent updateTopicEvent)
    {
        var comment = await commentStorage
            .GetCommentByIdAsync(updateTopicEvent.Id);
        
        if (comment is null) return;

        comment.MessageText = updateTopicEvent.Text;
        comment.CreatedAt = updateTopicEvent.UpdateDate;
        comment.IsTextEdited = true;
        
        await commentStorage.UpdateCommentAsync(comment);
    }

    public async Task On(RemoveCommentEvent removeCommentEvent)
    {
        await commentStorage.DeleteCommentAsync(removeCommentEvent.Id);
    }
}