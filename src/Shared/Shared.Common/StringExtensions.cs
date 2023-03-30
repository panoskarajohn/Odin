namespace Shared.Common;

public static class StringExtensions
{
    public static string Underscore(this string value)
    {
        return string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString()))
            .ToLowerInvariant();
    }
}