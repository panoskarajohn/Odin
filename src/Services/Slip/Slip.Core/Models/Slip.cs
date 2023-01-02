using Shared.Domain;
using Slip.Core.Exceptions;
using Slip.Core.ValueObjects;

namespace Slip.Core.Models;

public class Slip 
{
    public Guid Id { get; }
    private readonly ISet<Bet> _bets = new HashSet<Bet>();
    
    private IEnumerable<Bet> Bets => _bets;

    public UserId UserId { get; set; }
    
    private Slip(Guid userId)
    {
        Id = userId;
    }
    
    public void AddBet(Bet bet)
    {
        if (_bets.Contains(bet))
            throw new DuplicateBetException();

        _bets.Add(bet);
    }

    public static Slip Create(Guid userId) => new(userId);
}