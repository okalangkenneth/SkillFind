using Microsoft.AspNetCore.Mvc;
using Nest;
using Search.Domain.Documents;

namespace Search.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<SearchController> _logger;

        public SearchController(IElasticClient elasticClient, ILogger<SearchController> logger)
        {
            _elasticClient = elasticClient;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<JobPostDocument>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<JobPostDocument>>> Search(
            [FromQuery] string? q,
            [FromQuery] string? category,
            [FromQuery] string? location,
            [FromQuery] int page = 1,
            [FromQuery] int size = 10)
        {
            var response = await _elasticClient.SearchAsync<JobPostDocument>(s => s
                .Query(query => query
                    .Bool(b => b
                        .Must(must =>
                            string.IsNullOrEmpty(q)
                                ? must.MatchAll()
                                : must.MultiMatch(mm => mm
                                    .Fields(f => f.Field(x => x.Title, 2.0).Field(x => x.Description))
                                    .Query(q)))
                        .Filter(
                            f => f.Term(t => t.IsActive, true),
                            f => string.IsNullOrEmpty(category) ? f.MatchAll() : f.Term(t => t.CategoryName, category),
                            f => string.IsNullOrEmpty(location) ? f.MatchAll() : f.Term(t => t.Location, location)
                        )
                    )
                )
                .From((page - 1) * size)
                .Size(size)
                .Sort(sort => sort.Descending(d => d.PostedAt)));

            _logger.LogInformation("Search for '{Query}' returned {Count} results", q, response.Total);
            return Ok(response.Documents);
        }
    }
}
