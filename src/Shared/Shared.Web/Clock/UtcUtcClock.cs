namespace Shared.Web.Clock;

public class UtcUtcClock : IUtcClock
{
    public DateTime Current()
    {
        return DateTime.UtcNow;
    }
}