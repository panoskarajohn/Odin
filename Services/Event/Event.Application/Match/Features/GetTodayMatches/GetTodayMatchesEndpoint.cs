using Event.Application.Match.Dtos;
using Event.Application.Match.Features.CreateMatch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Web;
using Swashbuckle.AspNetCore.Annotations;

namespace Event.Application.Match.Features.GetTodayMatches;

[Route(BaseApiPath + "/match")]
public class GetTodayMatchesEndpoint : BaseController
{

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Get matches for current day", Description = "Get matches for current day")]
    public async Task<ActionResult<IEnumerable<MatchesResponseDto>>> Get(CancellationToken cancellationToken)
    {
        var query = new GetTodayMatchesQuery();
        var result = await Dispatcher.QueryAsync(query, cancellationToken);
        return Ok(result);
    }

}