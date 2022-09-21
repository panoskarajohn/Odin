namespace Shared.Web.ErrorHandling;

public interface IExceptionCompositionRoot
{
    ExceptionResponse Map(System.Exception exception);
}