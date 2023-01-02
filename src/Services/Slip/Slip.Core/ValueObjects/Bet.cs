using Slip.Core.Exceptions;
using Slip.Core.Models;

namespace Slip.Core.ValueObjects;

public record Bet()
{
    private ISet<BetSelection> _betSelections = new HashSet<BetSelection>();
    public Guid BetId { get; } = Guid.NewGuid();
    public Stake Stake { get; private set; } = 1.0m;
    public IEnumerable<BetSelection> Selections => _betSelections;

    public int NumberOfSelections => Selections.Count();

    public string BetType => NumberOfSelections >= 4
        ? $"{Enumerations.BetType.From(NumberOfSelections)}_{NumberOfSelections}"
        : Enumerations.BetType.From(NumberOfSelections).ToString();
    
    public decimal Winnings => Stake * Selections.Aggregate(1.0m, (acc, val) => acc * val.Odds);

    public Bet WithStake(Stake stake)
    {
        this.Stake = stake;
        return this;
    }
    
    public void AddSelection(BetSelection selection)
    {
        var isSelectionDuplicate = _betSelections.Contains(selection) 
                                   || _betSelections.Any(bs => bs.EventId == selection.EventId);
        
        if (isSelectionDuplicate)
            throw new DuplicateBetSelectionException(); 
        
        _betSelections.Add(selection);
    }

    public static Bet Create() => new Bet();
}