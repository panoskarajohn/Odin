namespace Shared.Types.Exceptions;

public abstract class OdinException : Exception
{
    protected OdinException(string message) : base(message)
    {
    }
}