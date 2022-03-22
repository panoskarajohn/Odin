namespace Shared.Web.ErrorHandling;

public interface IExceptionToResponseMapper
{
    ExceptionResponse Map(System.Exception exception);
}