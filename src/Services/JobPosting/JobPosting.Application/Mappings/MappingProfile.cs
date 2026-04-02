using JobPosting.Application.Features.JobPostings.Commands.CreateJobPosting;
using JobPosting.Application.Features.JobPostings.Commands.UpdateJobPosting;
using JobPosting.Application.Features.JobPostings.Queries.GetJobPostingsList;
using JobPosting.Domain.Entities;
using Mapster;

namespace JobPosting.Application.Mappings
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Job_Posting, JobPostingsVm>();
            config.NewConfig<JobPostingsVm, Job_Posting>();
            config.NewConfig<CreateJobPostingCommand, Job_Posting>();
            config.NewConfig<Job_Posting, CreateJobPostingCommand>();
            config.NewConfig<UpdateJobPostingCommand, Job_Posting>();
            config.NewConfig<Job_Posting, UpdateJobPostingCommand>();
        }
    }
}
