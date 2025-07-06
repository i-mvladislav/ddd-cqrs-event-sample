using Core.Handlers;
using Topic.CommandService.Api.Topic.Commands.CreateTopic;
using Topic.CommandService.Api.Topic.Commands.LikeTopic;
using Topic.CommandService.Api.Topic.Commands.RemoveTopic;
using Topic.CommandService.Api.Topic.Commands.UpdateTopic;
using Topic.CommandService.Domain.Aggregates;

namespace Topic.CommandService.Api.Topic.Commands;

public class TopicCommandHandler(IEventHandler<ContentAggregate> eventHandler)
    : ITopicCommandHandler
{
    public async Task HandleAsync(CreateTopicCommand command, CancellationToken ct)
    {
        var aggregate = new ContentAggregate(
            command.Id,
            command.AuthorName,
            command.MessageText
        );
        await eventHandler.SaveAsync(aggregate, ct);
    }

    public async Task HandleAsync(RemoveTopicCommand command, CancellationToken ct)
    {
        var aggregate = await eventHandler.GetAggregateByIdAsync(command.Id, ct);
        aggregate.RemoveTopic(command.AuthorName);
        await eventHandler.SaveAsync(aggregate, ct);
    }

    public async Task HandleAsync(UpdateTopicCommand command, CancellationToken ct)
    {
        var aggregate = await eventHandler.GetAggregateByIdAsync(command.Id, ct);
        aggregate.UpdateTopic(command.MessageText);
        await eventHandler.SaveAsync(aggregate, ct);
    }

    public async Task HandleAsync(LikeTopicCommand command, CancellationToken ct)
    {
        var aggregate = await eventHandler.GetAggregateByIdAsync(command.Id, ct);
        aggregate.LikeTopic();
        await eventHandler.SaveAsync(aggregate, ct);
    }
}