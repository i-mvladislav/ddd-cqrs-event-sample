using Confluent.Kafka;
using Core.MediatR;
using Microsoft.EntityFrameworkCore;
using Topic.QueryService.Api.Queries;
using Topic.QueryService.Api.Queries.GetTopicById;
using Topic.QueryService.Api.Queries.GetTopics;
using Topic.QueryService.Api.Queries.GetTopicsByAuthorName;
using Topic.QueryService.Api.Queries.GetTopicsWithComments;
using Topic.QueryService.Api.Queries.GetTopicsWithLikes;
using Topic.QueryService.Domain.Dao;
using Topic.QueryService.Domain.Entities;
using Topic.QueryService.Infrastructure.Consumers;
using Topic.QueryService.Infrastructure.Dao;
using Topic.QueryService.Infrastructure.Data;
using Topic.QueryService.Infrastructure.Handlers;
using Topic.QueryService.Infrastructure.MediatR;

namespace Topic.QueryService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddQueryServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContextWithFactory(configuration);

        services.AddScoped<ITopicStorage, TopicStorage>();
        services.AddScoped<ICommentStorage, CommentStorage>();
        services.AddScoped<IQueryEventHandler, QueryEventHandler>();

        var consumerConfig = new ConsumerConfig();
        configuration.GetSection(nameof(ConsumerConfig)).Bind(consumerConfig);
        services.AddSingleton(consumerConfig);

        services.AddScoped<IKafkaEventSubscriber, KafkaEventSubscriber>();
        services.AddHostedService<KafkaEventConsumerBackgroundService>();

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        DatabaseInitializer.Initialize(app);

        return app;
    }

    public static IServiceCollection AddDbContextWithFactory(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PgConnection");
        
        Action<DbContextOptionsBuilder> config = options => options
            .UseLazyLoadingProxies()
            .UseNpgsql(connectionString);

        services.AddSingleton(new DbContextFactory(config));
        services.AddDbContext<ApplicationContext>(config);

        return services;
    }

    private static IServiceCollection RegisterQueriesHandler(
        this IServiceCollection services)
    {
        services.AddScoped<ITopicQueryHandler, TopicQueryHandler>();

        services.AddScoped<IQueryDispatcher<TopicEntity>>(provider =>
        {
            var dispatcher = new QueryDispatcher();
            
            var topicQueryHandler = provider
                .GetRequiredService<ITopicQueryHandler>();
            
            dispatcher.RegisterHandler<GetTopicsQuery>(query => 
                topicQueryHandler.HandleAsync(query));
            dispatcher.RegisterHandler<GetTopicByIdQuery>(query => 
                topicQueryHandler.HandleAsync(query));
            dispatcher.RegisterHandler<GetTopicsByAuthorNameQuery>(query => 
                topicQueryHandler.HandleAsync(query));
            dispatcher.RegisterHandler<GetTopicsWithCommentsQuery>(query => 
                topicQueryHandler.HandleAsync(query));
            dispatcher.RegisterHandler<GetTopicsWithLikesQuery>(query => 
                topicQueryHandler.HandleAsync(query));
        }); 
        
        return services;
    }
}