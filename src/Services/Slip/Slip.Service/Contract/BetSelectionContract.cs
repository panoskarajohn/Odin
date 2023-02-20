namespace Slip.Service.Contract;

public class BetSelectionContract
{
    public long Id { get; set; }
    public long EventId { get; set; }
    public string MarketName { get; set; }
    public string Outcome { get; set; }
    public decimal Odds { get; set; }
}