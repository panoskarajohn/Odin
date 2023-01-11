using Slip.Core.Exceptions;
using Slip.Core.Models;

namespace Slip.Core.ValueObjects;

public record Bet
{
    private const int MaxNumberOfSelections = 25;
    private ISet<BetSelection> _betSelections = new HashSet<BetSelection>();
    private Stake _stake = 1.0m;
    public decimal Stake => _stake;
    public IEnumerable<BetSelection> Selections => _betSelections;

    public int NumberOfSelections => Selections.Count();

    public string BetType => NumberOfSelections >= 4
        ? $"{Enumerations.BetType.Accumulator}_{NumberOfSelections}"
        : Enumerations.BetType.From(NumberOfSelections).ToString();

    public decimal Winnings => Selections.Aggregate(1.0m, (acc, val) => acc * val.Odds) * Stake;

    private Bet()
    {
        
    }
    
    public Bet WithStake(Stake stake)
    {
        _stake = stake;
        return this;
    }
    
    public void AddSelection(BetSelection selection)
    {
        var isSelectionDuplicate = _betSelections.Contains(selection) 
                                   || _betSelections.Any(bs => bs.EventId == selection.EventId);

        if (_betSelections.Count > MaxNumberOfSelections)
            throw new BetSelectionLimitReachedException(MaxNumberOfSelections);
            
        
        if (isSelectionDuplicate)
            throw new DuplicateBetSelectionException(); 
        
        _betSelections.Add(selection);
    }
    
    public static Bet Create() => new Bet();
}