﻿using Backoffice.Models;

namespace Backoffice.ApiServices;

public interface IEventService
{
    Task<Event> GetEventAsync(int id);
}