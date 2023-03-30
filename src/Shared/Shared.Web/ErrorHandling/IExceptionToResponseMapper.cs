namespace Shared.Web.ErrorHandling;

public interface IExceptionToResponseMapper
{
    ExceptionResponse Map(Exception exception);
}