namespace Slip.Service.Domain;

public class Bet
{
    public long Id { get; set; }

    public decimal Stake { get; set; }
    public string BetType { get; set; }
    public decimal Winnings { get; set; }
    public int NumberOfSelections { get; set; }

    public List<BetSelection> Selections { get; set; }
    public string BetStatus { get; set; }
    public Slip Slip { get; set; }
    public string SlipId { get; set; }
}