using JobSeeker.Application.Features.JobSeekers.Commands.CreateJobSeeker;
using JobSeeker.Application.Features.JobSeekers.Commands.UpdateJobSeeker;
using JobSeeker.Application.Features.JobSeekers.Queries.GetJobSeekerById;
using JobSeeker.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobSeeker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobSeekersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobSeekersController(IMediator mediator) => _mediator = mediator;

        [HttpGet("{id}", Name = "GetJobSeekerById")]
        [ProducesResponseType(typeof(JobSeekerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JobSeekerDto>> GetById(Guid id) =>
            Ok(await _mediator.Send(new GetJobSeekerByIdQuery(id)));

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateJobSeekerCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtRoute("GetJobSeekerById", new { id }, id);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateJobSeekerCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
