using Core.MediatR;
using Topic.QueryService.Api.Queries.GetTopicsWithLikes;
using Topic.QueryService.Api.ResponseDtos;
using Topic.QueryService.Domain.Entities;

namespace Topic.QueryService.Api.Topics.Endpoints;

public class GetTopicsWithLikesEndpoint: BaseQueryEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/topics/likes/{count}", async (
            int count,
            ILogger<GetTopicsWithLikesEndpoint> logger,
            IQueryDispatcher<TopicEntity> queryDispatcher
            ) =>
        {
            return await ExecuteQueryAsync(
                logger,
                async () => await queryDispatcher.SendAsync(
                    new GetTopicsWithLikesQuery { Count = count }
                ),
                "Ошибка запроса"
            );
        })
        .Produces<QueryTopicsResponse>()
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}