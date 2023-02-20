using Slip.Service.Domain;

namespace Slip.Service.Contract;

public class BetContract
{
    public long Id { get; set; }
    public decimal Stake { get; set; }
    public string BetType { get; set; }
    public decimal Winnings { get; set; }
    public int NumberOfSelections { get; set; }
    public List<BetSelectionContract> Selections { get; set; }
    public string BetStatus { get; set; } = "New";
}