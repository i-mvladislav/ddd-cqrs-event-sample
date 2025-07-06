using Core.MediatR;
using Microsoft.AspNetCore.Mvc;
using Topic.CommandService.Api.Comments.Commands.CreateComment;
using Topic.CommandService.Api.Endpoints;
using Topic.CommandService.Api.ResponseDtos;

namespace Topic.CommandService.Api.Comments.Endpoints;

public class CreateCommentEndpoint: BaseEndpoint<CreateCommentCommand>
{
    protected override string GetSuccessMessage => "Комментарий успешно создан";
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/topics/{topicId:guid}/comments", async (
            Guid topicId,
            [FromBody] CreateCommentCommand command,
            ICommandDispatcher commandDispatcher,
            ILogger<CreateCommentEndpoint> logger
            ) =>
        {
            command.Id = topicId;
            return await ExecuteCommandAsync(command,
                cmd => commandDispatcher.SendCommandAsync(cmd),
                logger);
        })
        .Produces<ResponseDto>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}