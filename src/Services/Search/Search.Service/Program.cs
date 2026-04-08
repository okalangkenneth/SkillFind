using MassTransit;
using Microsoft.OpenApi.Models;
using Nest;
using Search.Domain.Documents;
using Search.Service.Consumers;
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
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(
            new Uri(context.Configuration["Elasticsearch:Uri"] ?? "http://localhost:9200"))
        {
            AutoRegisterTemplate = true,
            IndexFormat = "search-logs-{0:yyyy.MM.dd}",
            NumberOfReplicas = 0,
            NumberOfShards = 1
        }));

    // Elasticsearch NEST client
    builder.Services.AddSingleton<IElasticClient>(_ =>
    {
        var uri = builder.Configuration["Elasticsearch:Uri"] ?? "http://localhost:9200";
        var settings = new ConnectionSettings(new Uri(uri))
            .DefaultIndex("skillfind-jobs")
            .DefaultMappingFor<JobPostDocument>(m => m.IndexName("skillfind-jobs"));
        return new ElasticClient(settings);
    });

    // MassTransit + RabbitMQ
    builder.Services.AddMassTransit(x =>
    {
        x.AddConsumer<JobPostCreatedIndexConsumer>();
        x.UsingRabbitMq((ctx, cfg) =>
        {
            cfg.Host(builder.Configuration["RabbitMQ:Host"] ?? "localhost", "/", h =>
            {
                h.Username(builder.Configuration["RabbitMQ:Username"] ?? "guest");
                h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
            });
            cfg.ConfigureEndpoints(ctx);
        });
    });

    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen(c =>
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Search.Service", Version = "v1" }));

    var app = builder.Build();

    app.MapGet("/healthz", () => Results.Ok(new { status = "healthy" }));

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Search.Service v1"));
    }

    app.UseRouting();
    app.UseAuthorization();
    app.MapControllers();

    // Ensure the jobs index exists with correct mapping
    var elasticClient = app.Services.GetRequiredService<IElasticClient>();
    var indexExists = await elasticClient.Indices.ExistsAsync("skillfind-jobs");
    if (!indexExists.Exists)
    {
        await elasticClient.Indices.CreateAsync("skillfind-jobs", c => c
            .Map<JobPostDocument>(m => m.AutoMap())
            .Settings(s => s
                .NumberOfShards(1)
                .NumberOfReplicas(0)));
        Log.Information("Created Elasticsearch index 'skillfind-jobs'");
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
