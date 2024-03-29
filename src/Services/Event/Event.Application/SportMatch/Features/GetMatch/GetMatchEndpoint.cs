﻿using Event.Application.SportMatch.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Web;
using Swashbuckle.AspNetCore.Annotations;

namespace Event.Application.SportMatch.Features.GetMatch;

[Route(BaseApiPath + "/match")]
public class GetMatchEndpoint : BaseController
{
    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Get match", Description = "Get match")]
    public async Task<ActionResult<MatchResponseDto>> Get([FromRoute] long id,
        CancellationToken cancellationToken)
    {
        var query = new GetMatchQuery(id);
        var result = await Dispatcher.QueryAsync(query, cancellationToken);
        return Ok(result);
    }
}