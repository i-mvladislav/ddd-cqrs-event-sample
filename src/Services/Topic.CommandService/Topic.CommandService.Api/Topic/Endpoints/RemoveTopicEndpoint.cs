using Core.MediatR;
using Microsoft.AspNetCore.Mvc;
using Topic.CommandService.Api.Endpoints;
using Topic.CommandService.Api.Topic.Commands.RemoveTopic;

namespace Topic.CommandService.Api.Topic.Endpoints;

public class RemoveTopicEndpoint: BaseEndpoint<RemoveTopicCommand>
{
    protected override string GetSuccessMessage => "Топик успешно удалён";
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/topics/{topicId:guid}", async (
            Guid topicId,
            [FromBody] RemoveTopicCommand command,
            ILogger<RemoveTopicEndpoint> logger,
            ICommandDispatcher commandDispatcher
            ) =>
        {
            command.Id = topicId;
            return await ExecuteCommandAsync(command,
                async cmd => await commandDispatcher.SendCommandAsync(cmd),
                logger);
        })
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}