using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Topic.QueryService.Infrastructure.Consumers;

public class KafkaEventConsumerBackgroundService(
    ILogger<KafkaEventConsumerBackgroundService> logger,
    IServiceProvider serviceProvider,
    IConfiguration configuration): IHostedService
{
    public async Task StartAsync(CancellationToken ct)
    {
        logger.LogInformation("Служба потребителей событий запущена");
        await using var scope = serviceProvider.CreateAsyncScope();
        var eventConsumer = scope
            .ServiceProvider
            .GetRequiredService<IKafkaEventSubscriber>();
        
        var topic = configuration.GetValue<string>("Kafka:Topic")!;
        Task.Run(() => eventConsumer.ConsumeAsync(topic, ct), ct);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation($"Служба потребителей событий остановлена");
        return Task.CompletedTask;
    }
}