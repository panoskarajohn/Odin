namespace Backoffice.Models;

public sealed class Market
{
    public string Name { get; set; }
    public List<Selection> Selections { get; set; }
}