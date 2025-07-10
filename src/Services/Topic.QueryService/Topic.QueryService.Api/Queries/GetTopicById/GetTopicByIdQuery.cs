using Core.Queries;

namespace Topic.QueryService.Api.Queries.GetTopicById;

public record GetTopicByIdQuery: BaseQuery
{
    public required Guid Id { get; init; }
}