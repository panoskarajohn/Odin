using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Web;
using Swashbuckle.AspNetCore.Annotations;

namespace Event.Application.SportMatch.Features.RemoveMarkets;

[Route(BaseApiPath + "/match/market/remove")]
public class RemoveMarketsEndpoint : BaseController
{
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Remove markets to match", Description = "Remove markets to match")]
    public async Task<IActionResult> RemoveMarkets(RemoveMarketsCommand command, CancellationToken cancellationToken)
    {
        await Dispatcher.SendAsync(command);
        return NoContent();
    }
}