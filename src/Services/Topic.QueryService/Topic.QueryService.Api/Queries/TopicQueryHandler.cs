using Topic.QueryService.Api.Queries.GetTopicById;
using Topic.QueryService.Api.Queries.GetTopics;
using Topic.QueryService.Api.Queries.GetTopicsByAuthorName;
using Topic.QueryService.Api.Queries.GetTopicsWithComments;
using Topic.QueryService.Api.Queries.GetTopicsWithLikes;
using Topic.QueryService.Domain.Dao;
using Topic.QueryService.Domain.Entities;

namespace Topic.QueryService.Api.Queries;

public class TopicQueryHandler(ITopicStorage topicStorage): ITopicQueryHandler
{
    public async Task<IEnumerable<TopicEntity>> HandleAsync(GetTopicsQuery query)
    {
        return await topicStorage.GetAllTopicsAsync();
    }

    public async Task<IEnumerable<TopicEntity>> HandleAsync(GetTopicByIdQuery query)
    {
        return [
            await topicStorage.GetTopicByIdAsync(query.Id)
        ];
    }

    public async Task<IEnumerable<TopicEntity>> HandleAsync(GetTopicsByAuthorNameQuery query)
    {
        return await topicStorage.GetTopicsByAuthorAsync(query.AuthorName);
    }

    public async Task<IEnumerable<TopicEntity>> HandleAsync(GetTopicsWithCommentsQuery query)
    {
        return await topicStorage.GetTopicsWithCommentsAsync();
    }

    public async Task<IEnumerable<TopicEntity>> HandleAsync(GetTopicsWithLikesQuery query)
    {
        return await topicStorage.GetTopicsWithMinLikesAsync(query.Count);
    }
}