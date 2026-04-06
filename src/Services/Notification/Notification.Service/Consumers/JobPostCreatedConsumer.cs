using MassTransit;
using Microsoft.Extensions.Logging;
using Notification.Domain.Events;

namespace Notification.Service.Consumers
{
    public class JobPostCreatedConsumer : IConsumer<JobPostCreatedEvent>
    {
        private readonly ILogger<JobPostCreatedConsumer> _logger;

        public JobPostCreatedConsumer(ILogger<JobPostCreatedConsumer> logger) => _logger = logger;

        public async Task Consume(ConsumeContext<JobPostCreatedEvent> context)
        {
            var job = context.Message;
            _logger.LogInformation(
                "New job posted: {Title} at {Company} in {Category}",
                job.Title, job.CompanyName, job.CategoryName);

            // TODO Phase 3: send email via SMTP or push notification
            await Task.CompletedTask;
        }
    }
}
