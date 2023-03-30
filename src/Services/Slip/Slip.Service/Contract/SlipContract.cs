namespace Slip.Service.Contract;

public class SlipContract
{
    public string Id { get; set; }
    public List<BetContract> Bets { get; set; }
    public string UserId { get; set; }
    public decimal TotalStake { get; set; }
}