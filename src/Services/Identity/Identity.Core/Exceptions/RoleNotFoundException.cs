using Shared.Types.Exceptions;

namespace Identity.Core.Exceptions;

public class RoleNotFoundException : OdinException
{
    public RoleNotFoundException() : base("Role not found exception")
    {
    }
}