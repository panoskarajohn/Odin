using Ardalis.GuardClauses;
using Backoffice.ApiServices;
using Backoffice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backoffice.Controllers;

[Authorize]
public class EventController : Controller
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = Guard.Against.Null(eventService);
    }

    public async Task<IActionResult> GetEvent(long id = 7331125649473536)
    {
        var @event = await _eventService.GetEventAsync(id);
        return View(@event);
    }
}