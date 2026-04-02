using JobPosting.Application.Features.JobPostings.Commands.CreateJobPosting;
using JobPosting.Application.Features.JobPostings.Commands.DeleteJobPosting;
using JobPosting.Application.Features.JobPostings.Commands.UpdateJobPosting;
using JobPosting.Application.Features.JobPostings.Queries.GetJobPostingsList;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace JobPosting.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class JobPostingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobPostingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{title}", Name = "GetJobPosting")]
        [ProducesResponseType(typeof(IEnumerable<JobPostingsVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<JobPostingsVm>>> GetJobPostingsByTitle(string title)
        {
            var query = new GetJobPostingsListQuery(title);
            var jobPostings = await _mediator.Send(query);
            return Ok(jobPostings);
        }

        [HttpPost(Name = "CreateJobPosting")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CreateJobPosting([FromBody] CreateJobPostingCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut(Name = "UpdateJobPosting")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateJobPosting([FromBody] UpdateJobPostingCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteJobPosting")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteJobPosting(int id)
        {
            var command = new DeleteJobPostingCommand() { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
