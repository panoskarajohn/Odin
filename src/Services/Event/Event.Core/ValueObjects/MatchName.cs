using Event.Core.Exceptions;

namespace Event.Core.ValueObjects;

public record MatchName
{
    private readonly string _away;

    private readonly string _home;

    public MatchName(string home, string away)
    {
        if (string.IsNullOrWhiteSpace(home)) throw new InvalidTeamException(nameof(home));
        if (string.IsNullOrWhiteSpace(away)) throw new InvalidTeamException(nameof(away));

        _home = home;
        _away = away;
    }

    public string Value => $"{_home} vs {_away}";

    public void Deconstruct(out string homeTeam, out string awayTeam)
    {
        homeTeam = _home;
        awayTeam = _away;
    }
}