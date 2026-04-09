using JobPosting.API.Services;
using JobPosting.Application;
using JobPosting.Application.Exceptions;
using JobPosting.Application.Interfaces;
using JobPosting.Infrastructure;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
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
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(
            new Uri(context.Configuration["Elasticsearch:Uri"] ?? "http://localhost:9200"))
        {
            AutoRegisterTemplate = true,
            IndexFormat = "jobposting-logs-{0:yyyy.MM.dd}",
            NumberOfReplicas = 0,
            NumberOfShards = 1
        }));

    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);

    builder.Services.AddMassTransit(x =>
    {
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

    // JWT Bearer auth — tokens issued by JobSeeker.API (OpenIddict)
    builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            options.Authority = builder.Configuration["Auth:Authority"];
            options.RequireHttpsMetadata = false; // dev/local only
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                // Audience validation disabled: OpenIddict does not embed a resource-server
                // audience by default without application seeding. Enable in production
                // once a resource server is registered in the OpenIddict database.
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1)
            };
        });
    builder.Services.AddAuthorization();

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

    builder.Services.AddControllers();
    builder.Services.AddHealthChecks()
        .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection")!);
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "JobPosting.API", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter your JWT token from POST /connect/token on JobSeeker.API"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                Array.Empty<string>()
            }
        });
    });

    var app = builder.Build();

    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            var error = context.Features.Get<IExceptionHandlerFeature>();
            if (error?.Error is ForbiddenException)
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { error = error.Error.Message });
            }
            else if (error?.Error is NotFoundException)
            {
                context.Response.StatusCode = 404;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { error = error.Error.Message });
            }
            else
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred." });
            }
        });
    });

    app.MapHealthChecks("/healthz");

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JobPosting.API v1"));
    }

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

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
