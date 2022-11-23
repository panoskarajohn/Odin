namespace Backoffice.Models;

public class Event
{
    public long Id { get; set; }
    public string Category { get; set; }
    public DateTime StartingTime { get; set; }
    public string MatchName { get; set; }
    public List<Market> Markets { get; set; }
}