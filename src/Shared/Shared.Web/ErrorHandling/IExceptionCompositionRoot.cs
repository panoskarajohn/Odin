namespace Shared.Web.ErrorHandling;

public interface IExceptionCompositionRoot
{
    ExceptionResponse Map(Exception exception);
}