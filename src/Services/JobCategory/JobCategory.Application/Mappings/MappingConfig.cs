using JobCategory.Application.Features.JobCategories.Commands.CreateJobCategory;
using JobCategory.Application.Features.JobCategories.Commands.UpdateJobCategory;
using JobCategory.Application.Models;
using Mapster;

namespace JobCategory.Application.Mappings
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<JobCategoryEntity, JobCategoryDto>();
            config.NewConfig<CreateJobCategoryCommand, JobCategoryEntity>();
            config.NewConfig<UpdateJobCategoryCommand, JobCategoryEntity>();
        }
    }
}
