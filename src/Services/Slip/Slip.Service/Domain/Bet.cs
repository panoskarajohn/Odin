using System.Text.Json.Serialization;
using Slip.Service.DAL;

namespace Slip.Service.Domain;

public class Bet
{
    public long Id { get; set; }
    
    public decimal Stake { get; set; }
    public string BetType { get; set; }
    public decimal Winnings { get; set; }
    public int NumberOfSelections { get; set; }
    
    public List<BetSelection> Selections { get; set; }
    [JsonIgnore]
    public virtual Slip Slip { get; set; }
    [JsonIgnore]
    public string SlipId { get; set; }
}