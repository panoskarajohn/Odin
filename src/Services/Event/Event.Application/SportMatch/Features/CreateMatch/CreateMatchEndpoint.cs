using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.IdGenerator;
using Shared.Web;
using Swashbuckle.AspNetCore.Annotations;

namespace Event.Application.SportMatch.Features.CreateMatch;

[Route(BaseApiPath + "/match")]
public class CreateMatchEndpoint : BaseController
{
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Create new match", Description = "Create new match")]
    public async Task<ActionResult> Create([FromBody] CreateMatchCommand command, CancellationToken cancellationToken)
    {
        //Not best since we always will create two objects
        command = command with {EventId = SnowFlakIdGenerator.NewId()};
        await Dispatcher.SendAsync(command, cancellationToken);
        return Created(BaseApiPath + "/match", new {command.EventId});
    }
}