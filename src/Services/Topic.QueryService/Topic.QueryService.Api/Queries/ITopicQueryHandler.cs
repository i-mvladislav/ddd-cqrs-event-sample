using Topic.QueryService.Api.Queries.GetTopicById;
using Topic.QueryService.Api.Queries.GetTopics;
using Topic.QueryService.Api.Queries.GetTopicsByAuthorName;
using Topic.QueryService.Api.Queries.GetTopicsWithComments;
using Topic.QueryService.Api.Queries.GetTopicsWithLikes;
using Topic.QueryService.Domain.Entities;

namespace Topic.QueryService.Api.Queries;

public interface ITopicQueryHandler
{
    Task<IEnumerable<TopicEntity>> HandleAsync(GetTopicsQuery query);
    Task<IEnumerable<TopicEntity>> HandleAsync(GetTopicByIdQuery query);
    Task<IEnumerable<TopicEntity>> HandleAsync(GetTopicsByAuthorNameQuery query);
    Task<IEnumerable<TopicEntity>> HandleAsync(GetTopicsWithCommentsQuery query);
    Task<IEnumerable<TopicEntity>> HandleAsync(GetTopicsWithLikesQuery query);
}