namespace Shared.Common;

public static class DateTimeExtensions
{
    /// <summary>
    /// Checks if dateTime is from the future
    /// </summary>
    /// <param name="dateTime"></param>
    public static bool UtcTimeIsFromFuture(this DateTime dateTime)
    {
        var dateTimeUtc = dateTime.ToUniversalTime();
        var utcNow = DateTime.UtcNow;
        return utcNow <= dateTimeUtc;
    }
}