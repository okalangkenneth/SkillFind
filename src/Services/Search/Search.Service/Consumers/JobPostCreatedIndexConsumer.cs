using MassTransit;
using Microsoft.Extensions.Logging;
using Nest;
using Notification.Domain.Events;
using Search.Domain.Documents;

namespace Search.Service.Consumers
{
    public class JobPostCreatedIndexConsumer : IConsumer<JobPostCreatedEvent>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<JobPostCreatedIndexConsumer> _logger;

        public JobPostCreatedIndexConsumer(IElasticClient elasticClient, ILogger<JobPostCreatedIndexConsumer> logger)
        {
            _elasticClient = elasticClient;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<JobPostCreatedEvent> context)
        {
            var job = context.Message;
            var doc = new JobPostDocument
            {
                Id = job.JobPostId,
                Title = job.Title,
                CompanyName = job.CompanyName,
                CategoryName = job.CategoryName,
                PostedAt = job.PostedAt,
                IsActive = true
            };

            var response = await _elasticClient.IndexDocumentAsync(doc);

            if (!response.IsValid)
                _logger.LogError("Failed to index job {Id}: {Error}", job.JobPostId, response.DebugInformation);
            else
                _logger.LogInformation("Indexed job {Id}: {Title}", job.JobPostId, job.Title);
        }
    }
}
