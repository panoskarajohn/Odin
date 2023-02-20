using Shared.Types.Exceptions;

namespace Event.Application.SportMatch.Exceptions;

public class TemplateNotFound : OdinException
{
    public TemplateNotFound(string marketName, string category) : base($"Template not found for marketName: {marketName} and category: {category}")
    {
    }
}