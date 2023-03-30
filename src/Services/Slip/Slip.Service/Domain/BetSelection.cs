namespace Slip.Service.Domain;

public class BetSelection
{
    public long Id { get; set; }
    public long EventId { get; set; }
    public string MarketName { get; set; }
    public string Outcome { get; set; }
    public decimal Odds { get; set; }

    public virtual Bet Bet { get; set; }
    public long BetId { get; set; }
}