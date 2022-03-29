using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Web;
using Swashbuckle.AspNetCore.Annotations;

namespace Event.Application.Match.Features.SuspendMatch;

[Route(BaseApiPath + "/match/suspend")]
public class SuspendMatchEndpoint : BaseController
{
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Suspend Match", Description = "Suspend match")]
    public async Task<ActionResult> Suspend([FromRoute] long id, CancellationToken cancellationToken)
    {
        var command = new SuspendMatchCommand(id);
        await Dispatcher.SendAsync(command, cancellationToken);
        return NoContent();
    }
}