using Topic.CommandService.Api.Topic.Commands.CreateTopic;
using Topic.CommandService.Api.Topic.Commands.LikeTopic;
using Topic.CommandService.Api.Topic.Commands.RemoveTopic;
using Topic.CommandService.Api.Topic.Commands.UpdateTopic;

namespace Topic.CommandService.Api.Topic;

public interface ITopicCommandHandler
{
    Task HandleAsync(CreateTopicCommand command, CancellationToken ct);
    Task HandleAsync(RemoveTopicCommand command, CancellationToken ct);
    Task HandleAsync(UpdateTopicCommand command, CancellationToken ct);
    Task HandleAsync(LikeTopicCommand command, CancellationToken ct);
}