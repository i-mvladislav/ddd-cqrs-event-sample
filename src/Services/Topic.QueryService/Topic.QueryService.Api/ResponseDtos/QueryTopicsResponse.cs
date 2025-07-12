using Core.ResponseDtos;
using Topic.QueryService.Domain.Entities;

namespace Topic.QueryService.Api.ResponseDtos;

public class QueryTopicsResponse: BaseResponse
{
    public IEnumerable<TopicEntity> Topics { get; set; } = null!;
}