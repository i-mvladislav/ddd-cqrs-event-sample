using Microsoft.EntityFrameworkCore;
using Topic.QueryService.Domain.Dao;
using Topic.QueryService.Infrastructure.Dao;
using Topic.QueryService.Infrastructure.Data;
using Topic.QueryService.Infrastructure.Handlers;

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
}