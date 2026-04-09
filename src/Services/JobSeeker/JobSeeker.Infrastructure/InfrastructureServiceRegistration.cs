using JobSeeker.Application.Contracts.Persistence;
using JobSeeker.Infrastructure.Persistence;
using JobSeeker.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;

namespace JobSeeker.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<JobSeekerDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                options.UseOpenIddict();
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<JobSeekerDbContext>()
                .AddDefaultTokenProviders();

            services.AddOpenIddict()
                .AddCore(options => options
                    .UseEntityFrameworkCore()
                    .UseDbContext<JobSeekerDbContext>())
                .AddServer(options =>
                {
                    options.SetTokenEndpointUris("/connect/token");
                    options.AllowPasswordFlow();
                    options.AllowRefreshTokenFlow();
                    // Register the skillfind-api scope so resource services can request it
                    options.RegisterScopes(OpenIddictConstants.Scopes.Email,
                                           OpenIddictConstants.Scopes.Profile,
                                           "skillfind-api");
                    // Disable encryption so standard JWT Bearer middleware in other services
                    // can validate tokens without sharing the encryption key
                    options.DisableAccessTokenEncryption();
                    // Allow the password flow without a registered client application
                    options.AcceptAnonymousClients();
                    options.AddDevelopmentEncryptionCertificate()
                           .AddDevelopmentSigningCertificate();
                    options.UseAspNetCore()
                           .EnableTokenEndpointPassthrough();
                })
                .AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseAspNetCore();
                });

            services.AddScoped<IJobSeekerRepository, JobSeekerRepository>();

            return services;
        }
    }
}
