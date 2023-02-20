using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Web;
using Swashbuckle.AspNetCore.Annotations;

namespace Event.Application.MarketTemplate.Commands.CreateNewMarketTemplate;

[Route(BaseApiPath + "/match/template")]
public class CreateTemplateEndpoint : BaseController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Create new market template", Description = "Create new market template")]
    public async Task<ActionResult> Create([FromBody] CreateMarketTemplateCommand command, CancellationToken cancellationToken)
    {
        await Dispatcher.SendAsync(command, cancellationToken);
        return NoContent();
    }
}