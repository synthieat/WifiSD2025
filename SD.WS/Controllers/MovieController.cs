using MediatR;
using Microsoft.AspNetCore.Mvc;
using SD.Core.Application.Queries;
using SD.Core.Application.Results;
using System.Collections;

namespace SD.WS.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class MovieController : Controller
    {
        private IMediator mediator;

        public MovieController()
        {        
        }

        private IMediator Mediator => mediator ??= HttpContext.RequestServices.GetService<IMediator>();


        [HttpGet(nameof(MovieDto))]
        public async Task<IEnumerable<MovieDto>> GetMovieDtos([FromQuery] GetMovieDtosQuery query , CancellationToken cancellationToken)
        {
            return await this.Mediator.Send(query, cancellationToken);
        }


    }
}
