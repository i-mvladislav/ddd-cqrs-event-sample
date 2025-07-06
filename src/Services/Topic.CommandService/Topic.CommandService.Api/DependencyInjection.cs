using Carter;
using Confluent.Kafka;
using Core.Events.Dao;
using Core.Handlers;
using Core.KafkaProducer;
using Core.MediatR;
using Core.Services;
using Marten;
using Topic.CommandService.Api.Comments;
using Topic.CommandService.Api.Comments.Commands.CreateComment;
using Topic.CommandService.Api.Comments.Commands.RemoveComment;
using Topic.CommandService.Api.Comments.Commands.UpdateComment;
using Topic.CommandService.Api.Topic;
using Topic.CommandService.Api.Topic.Commands;
using Topic.CommandService.Api.Topic.Commands.CreateTopic;
using Topic.CommandService.Api.Topic.Commands.LikeTopic;
using Topic.CommandService.Api.Topic.Commands.RemoveTopic;
using Topic.CommandService.Api.Topic.Commands.UpdateTopic;
using Topic.CommandService.Domain.Aggregates;
using Topic.CommandService.Infrastructure.Handlers;
using Topic.CommandService.Infrastructure.KafkaProducer;
using Topic.CommandService.Infrastructure.MediatR;
using Topic.CommandService.Infrastructure.Services;

namespace Topic.CommandService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddCommandServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("PgConnection")!;
        services
            .AddMarten(opt => opt.Connection(connectionString))
            .UseLightweightSessions();

        services.AddScoped<IEventStorage, EventStorage>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IEventHandler<ContentAggregate>, EventHandlerImpl>();
        services.AddScoped<ICommentCommandHandler, CommentCommandHandler>();
        services.AddScoped<ITopicCommandHandler, TopicCommandHandler>();
        services.RegisterCommandHandler();

        services.Configure<ProducerConfig>(configuration.GetSection("KafkaConfig"));
        services.AddScoped<IEventKafkaProducer, EventKafkaProducer>();

        services.AddCarter();
        
        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.MapCarter();
        return app;
    }

    private static IServiceCollection RegisterCommandHandler(
        this IServiceCollection services)
    {
        services.AddScoped<ICommandDispatcher>(provider =>
        {
            var dispatcher = new CommandDispatcher();
            
            var commandCommentHandler = provider.GetRequiredService<ICommentCommandHandler>();
            dispatcher.RegisterHandler<CreateCommentCommand>(command => commandCommentHandler.HandleAsync(command, CancellationToken.None));
            dispatcher.RegisterHandler<UpdateCommentCommand>(command => commandCommentHandler.HandleAsync(command, CancellationToken.None));
            dispatcher.RegisterHandler<RemoveCommentCommand>(command => commandCommentHandler.HandleAsync(command, CancellationToken.None));
            
            var commandTopicHandler = provider.GetRequiredService<ITopicCommandHandler>();
            dispatcher.RegisterHandler<CreateTopicCommand>(command => commandTopicHandler.HandleAsync(command, CancellationToken.None));
            dispatcher.RegisterHandler<RemoveTopicCommand>(command => commandTopicHandler.HandleAsync(command, CancellationToken.None));
            dispatcher.RegisterHandler<UpdateTopicCommand>(command => commandTopicHandler.HandleAsync(command, CancellationToken.None));
            dispatcher.RegisterHandler<LikeTopicCommand>(command => commandTopicHandler.HandleAsync(command, CancellationToken.None));

            return dispatcher;
        });

        return services;
    }
}