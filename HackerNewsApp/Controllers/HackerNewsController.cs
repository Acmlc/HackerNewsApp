using HackerNewsApp.Models;
using HackerNewsApp.Queries;
using HackerNewsApp.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace HackerNewsApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class HackerNewsController : ControllerBase
    {
        private readonly ILogger<HackerNewsController> _logger;
        private IMemoryCache _cache;
        private readonly IHackerNewsService _service;
        private readonly IMediator _mediator;

        public HackerNewsController(ILogger<HackerNewsController> logger, IMemoryCache cache, IHackerNewsService service, IMediator mediator)
        {
            _logger = logger;
            _cache = cache;
            _service = service;
            _mediator = mediator;
        }

        /// <summary>
        /// Get newest hacker news stories
        /// </summary>
        /// <param name="requestParameters"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetHackerNews_NewestStories")]
        [SwaggerOperation(OperationId = "HackerNews_GetNewestStories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<HackerNewsStory>>> Get([FromQuery] QueryParams requestParameters)
        {
            try {
                List<HackerNewsStory?> result = await _mediator.Send(new GetHackerNewsByName(requestParameters));
                return Ok(result);
            }
            catch (Exception ex){

                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
