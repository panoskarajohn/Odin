namespace Shared.Web.Clock;

public class UtcUtcClock : IUtcClock
{
    public DateTime Current() => DateTime.UtcNow;
}