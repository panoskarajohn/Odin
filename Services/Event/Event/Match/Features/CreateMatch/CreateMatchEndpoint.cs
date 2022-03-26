using Event.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Web;
using Swashbuckle.AspNetCore.Annotations;

namespace Event.Match.Features.CreateMatch;

[Route(BaseApiPath + "/match")]
public class CreateMatchEndpoint : BaseController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Create new aircraft", Description = "Create new aircraft")]
    public async Task<IActionResult> Create(CreateMatchCommand command, CancellationToken cancellationToken)
    {
        await Dispatcher.SendAsync(command, cancellationToken);
        return NoContent();
    }
}
