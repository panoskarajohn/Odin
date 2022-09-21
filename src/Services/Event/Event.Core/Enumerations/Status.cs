using Event.Core.Exceptions;
using Shared.Domain;

namespace Event.Core.Enumerations;

public class Status : Enumeration
{
    public static Status Active = new Status(1, nameof(Active).ToLowerInvariant());
    public static Status Suspended = new Status(2, nameof(Suspended).ToLowerInvariant());
    public static Status Completed = new Status(3, nameof(Completed).ToLowerInvariant());
    
    private Status(int id, string name) : base(id, name)
    {
    }

    public static IEnumerable<Status> List() => new[] {Active, Suspended, Completed};

    public static Status FromName(string name)
    {
        var state = List()
            .SingleOrDefault(s 
                => string
                    .Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));
        
        if (state == null)
        {
            throw new StatusInvalidStateException(List());
        }
        
        return state;
    }
    
    public static Status From(int id)
    {
        var state = List().SingleOrDefault(s => s.Id == id);

        if (state is null)
        {
            throw new StatusInvalidStateException(List());
        }

        return state;
    }
}