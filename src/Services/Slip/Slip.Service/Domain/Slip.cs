namespace Slip.Service.Domain;

public class Slip
{
    public string Id { get; set; }
    public List<Bet> Bets { get; set; }
    public string UserId { get; set; }
    public decimal TotalStake { get; set; }
}