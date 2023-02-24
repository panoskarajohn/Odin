using Shared.Types.Exceptions;

namespace Shared.Web.Exceptions;

public class ServiceNotFoundException : OdinException
{
    public ServiceNotFoundException(string service) : base($"Service: '{service}' was not found.")
    {
    }
}
