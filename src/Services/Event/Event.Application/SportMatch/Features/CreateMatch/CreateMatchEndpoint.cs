using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Web;
using Swashbuckle.AspNetCore.Annotations;

namespace Event.Application.SportMatch.Features.CreateMatch;

[Route(BaseApiPath + "/match")]
public class CreateMatchEndpoint : BaseController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Create new match", Description = "Create new match")]
    public async Task<ActionResult> Create([FromBody] CreateMatchCommand command, CancellationToken cancellationToken)
    {
        await Dispatcher.SendAsync(command, cancellationToken);
        return NoContent();
    }
}