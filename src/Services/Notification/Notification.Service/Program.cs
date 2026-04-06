using MassTransit;
using Microsoft.Extensions.Hosting;
using Notification.Service.Consumers;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var host = Host.CreateDefaultBuilder(args)
        .UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(
                new Uri(context.Configuration["Elasticsearch:Uri"] ?? "http://localhost:9200"))
            {
                AutoRegisterTemplate = true,
                IndexFormat = "notification-logs-{0:yyyy.MM.dd}",
                NumberOfReplicas = 0,
                NumberOfShards = 1
            }))
        .ConfigureServices((context, services) =>
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<JobPostCreatedConsumer>();
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(context.Configuration["RabbitMQ:Host"] ?? "localhost", "/", h =>
                    {
                        h.Username(context.Configuration["RabbitMQ:Username"] ?? "guest");
                        h.Password(context.Configuration["RabbitMQ:Password"] ?? "guest");
                    });
                    cfg.ConfigureEndpoints(ctx);
                });
            });
        })
        .Build();

    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
