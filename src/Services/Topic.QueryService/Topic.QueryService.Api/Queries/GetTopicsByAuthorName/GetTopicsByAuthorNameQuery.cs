using Core.Queries;

namespace Topic.QueryService.Api.Queries.GetTopicsByAuthorName;

public record GetTopicsByAuthorNameQuery: BaseQuery
{
    public required string AuthorName { get; init; }
}