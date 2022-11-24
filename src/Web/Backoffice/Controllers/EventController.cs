﻿using Ardalis.GuardClauses;
using Backoffice.ApiServices;
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

    public IActionResult GetEvent(long id)
    {
        return View();
    }
}