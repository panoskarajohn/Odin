using Slip.Core.Exceptions;
using Slip.Core.ValueObjects;

namespace Slip.Core.Models;

public class Slip
{
    private readonly ISet<Bet> _bets = new HashSet<Bet>();
    private readonly UserId _userId;

    private Slip(Guid userId, Guid id)
    {
        _userId = userId;
        Id = id == default ? Guid.NewGuid() : id;
    }

    public Guid Id { get; }
    public IEnumerable<Bet> Bets => _bets;

    public string UserId => _userId;

    public decimal TotalStake => _bets.Sum(bet => bet.Stake);

    public void AddBet(Bet bet)
    {
        if (_bets.Contains(bet))
            throw new DuplicateBetException();

        _bets.Add(bet);
    }

    public static Slip Create(Guid userId, Guid id = default)
    {
        return new(userId, id);
    }
}