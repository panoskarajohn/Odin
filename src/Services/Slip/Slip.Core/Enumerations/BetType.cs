using Shared.Domain;
using Slip.Core.Exceptions;

namespace Slip.Core.Enumerations;

public class BetType : Enumeration
{
    public static BetType Single = new BetType(1, nameof(Single).ToLowerInvariant());
    public static BetType Double = new BetType(2, nameof(Double).ToLowerInvariant());
    public static BetType Treble = new BetType(3, nameof(Treble).ToLowerInvariant());
    public static BetType Accumulator = new BetType(4, nameof(Accumulator).ToLowerInvariant());
    
    private BetType(int id, string name) : base(id, name)
    {
    }

    public static IEnumerable<BetType> List() => new[]
    {
        Single, 
        Double, 
        Treble,
        Accumulator
    };

    public static BetType FromName(string name)
    {
        var state = List()
            .SingleOrDefault(s 
                => string
                    .Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));
        
        if (state == null)
        {
            throw new BetTypeInvalidStateException(List());
        }
        
        return state;
    }
    
    public static BetType From(int id)
    {
        var state = List().SingleOrDefault(s => s.Id == id);

        if (state is null)
        {
            throw new BetTypeInvalidStateException(List());
        }

        return state;
    }
}