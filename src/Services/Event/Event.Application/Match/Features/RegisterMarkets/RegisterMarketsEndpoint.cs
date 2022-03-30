using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Web;
using Swashbuckle.AspNetCore.Annotations;

namespace Event.Application.Match.Features.RegisterMarkets;

[Route(BaseApiPath + "/match/market")]
public class RegisterMarketsEndpoint : BaseController
{
    [HttpPut()]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Append markets to match", Description = "Append markets to match")]
    public async Task<IActionResult> RegisterMarkets(RegisterMarketsCommand command,
        CancellationToken cancellationToken)
    {
        await Dispatcher.SendAsync(command);
        return NoContent();
    }
}