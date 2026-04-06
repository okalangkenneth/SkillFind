using JobCategory.Application.Features.JobCategories.Commands.CreateJobCategory;
using JobCategory.Application.Features.JobCategories.Commands.DeleteJobCategory;
using JobCategory.Application.Features.JobCategories.Commands.UpdateJobCategory;
using JobCategory.Application.Features.JobCategories.Queries.GetAllCategories;
using JobCategory.Application.Features.JobCategories.Queries.GetCategoryById;
using JobCategory.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobCategory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [ProducesResponseType(typeof(List<JobCategoryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<JobCategoryDto>>> GetAll() =>
            Ok(await _mediator.Send(new GetAllCategoriesQuery()));

        [HttpGet("{id}", Name = "GetCategoryById")]
        [ProducesResponseType(typeof(JobCategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JobCategoryDto>> GetById(int id) =>
            Ok(await _mediator.Send(new GetCategoryByIdQuery(id)));

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<int>> Create([FromBody] CreateJobCategoryCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtRoute("GetCategoryById", new { id }, id);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateJobCategoryCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteJobCategoryCommand { Id = id });
            return NoContent();
        }
    }
}
