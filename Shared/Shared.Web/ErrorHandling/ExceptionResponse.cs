using System.Net;

namespace Shared.Web.ErrorHandling;

public record ExceptionResponse(object Response, HttpStatusCode StatusCode);