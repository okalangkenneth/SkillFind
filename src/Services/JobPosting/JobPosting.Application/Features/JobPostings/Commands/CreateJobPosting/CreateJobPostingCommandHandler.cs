using JobPosting.Application.Contracts.Infrastructure;
using JobPosting.Application.Contracts.Persistence;
using JobPosting.Application.Models;
using JobPosting.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JobPosting.Application.Features.JobPostings.Commands.CreateJobPosting
{
    public class CreateJobPostingCommandHandler : IRequestHandler<CreateJobPostingCommand, int>
    {
        private readonly IJobPostingRepository _jobPostingRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<CreateJobPostingCommandHandler> _logger;

        public CreateJobPostingCommandHandler(IJobPostingRepository jobPostingRepository, IEmailService emailService, ILogger<CreateJobPostingCommandHandler> logger)
        {
            _jobPostingRepository = jobPostingRepository;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<int> Handle(CreateJobPostingCommand request, CancellationToken cancellationToken)
        {
            var jobPostingEntity = request.Adapt<Job_Posting>();
            var newJobPosting = await _jobPostingRepository.AddAsync(jobPostingEntity);

            _logger.LogInformation("Job Posting {Id} successfully created.", newJobPosting.Id);

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
