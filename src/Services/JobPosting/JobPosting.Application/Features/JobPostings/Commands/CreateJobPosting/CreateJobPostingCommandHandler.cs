using JobPosting.Application.Contracts.Infrastructure;
using JobPosting.Application.Contracts.Persistence;
using JobPosting.Application.Interfaces;
using JobPosting.Application.Models;
using JobPosting.Domain.Entities;
using Mapster;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Notification.Domain.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JobPosting.Application.Features.JobPostings.Commands.CreateJobPosting
{
    public class CreateJobPostingCommandHandler : IRequestHandler<CreateJobPostingCommand, int>
    {
        private readonly IJobPostingRepository _jobPostingRepository;
        private readonly IEmailService _emailService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<CreateJobPostingCommandHandler> _logger;
        private readonly ICurrentUserService _currentUser;

        public CreateJobPostingCommandHandler(
            IJobPostingRepository jobPostingRepository,
            IEmailService emailService,
            IPublishEndpoint publishEndpoint,
            ILogger<CreateJobPostingCommandHandler> logger,
            ICurrentUserService currentUser)
        {
            _jobPostingRepository = jobPostingRepository;
            _emailService = emailService;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
            _currentUser = currentUser;
        }

        public async Task<int> Handle(CreateJobPostingCommand request, CancellationToken cancellationToken)
        {
            var jobPostingEntity = request.Adapt<Job_Posting>();
            // Stamp the authenticated employer's identity — never trust client-supplied values
            jobPostingEntity.EmployerId = _currentUser.UserId;
            jobPostingEntity.EmployerEmail = _currentUser.Email;
            var newJobPosting = await _jobPostingRepository.AddAsync(jobPostingEntity);

            _logger.LogInformation("Job Posting {Id} successfully created.", newJobPosting.Id);

            await _publishEndpoint.Publish(new JobPostCreatedEvent
            {
                JobPostId = Guid.NewGuid(),
                Title = newJobPosting.Title,
                CompanyName = newJobPosting.CompanyName,
                CategoryName = newJobPosting.JobCategory ?? string.Empty,
                PostedAt = DateTime.UtcNow
            }, cancellationToken);

            await SendMail(newJobPosting);

            return newJobPosting.Id;
        }

        private async Task SendMail(Job_Posting jobPosting)
        {
            var email = new Email { To = jobPosting.ContactEmail, Body = "Job Posting was created.", Subject = "Job Posting was created" };

            try
            {
                await _emailService.SendEmail(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Job Posting {Id} notification email failed.", jobPosting.Id);
            }
        }
    }
}
