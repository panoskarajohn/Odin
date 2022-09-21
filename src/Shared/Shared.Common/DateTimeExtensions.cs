using NodaTime;

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
        return utcNow > dateTimeUtc;
    }

    /// <summary>
    /// Converts UTC to timezone 
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="timezone"></param>
    /// <returns></returns>
    public static DateTime UtcToTimezone(this DateTime dateTime, string timezone)
    {
        if (string.IsNullOrEmpty(timezone))
            throw new ArgumentNullException(nameof(timezone));
        
        if (dateTime.Kind == DateTimeKind.Utc)
            throw new InvalidOperationException("Datetime is not of Universal time zone");

        var dateTimeZone = DateTimeZoneProviders.Tzdb[timezone];
        var instant = Instant.FromDateTimeUtc(dateTime);
        var result = instant.InZone(dateTimeZone).ToDateTimeUnspecified();
        return result;

    }
}