using JobSeeker.Application.Models;
using Mapster;

namespace JobSeeker.Application.Mappings
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<JobSeekerEntity, JobSeekerDto>();
        }
    }
}
