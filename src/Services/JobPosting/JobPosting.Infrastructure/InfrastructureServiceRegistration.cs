using JobPosting.Application.Contracts.Infrastructure;
using JobPosting.Application.Contracts.Persistence;
using JobPosting.Application.Models;
using JobPosting.Infrastructure.Mail;
using JobPosting.Infrastructure.Persistence;
using JobPosting.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobPosting.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            System.AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddDbContext<JobPostingContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<IJobPostingRepository, JobPostingRepository>();

            services.Configure<EmailSettings>(opts => configuration.GetSection("EmailSettings").Bind(opts));
            services.AddTransient<IEmailService, EmailService>();

            return services;
        }
    }
}
