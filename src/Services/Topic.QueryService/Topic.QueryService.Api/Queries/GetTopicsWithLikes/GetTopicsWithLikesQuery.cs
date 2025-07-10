using Core.Queries;

namespace Topic.QueryService.Api.Queries.GetTopicsWithLikes;

public record GetTopicsWithLikesQuery: BaseQuery
{
    public required int Count { get; init; }
}