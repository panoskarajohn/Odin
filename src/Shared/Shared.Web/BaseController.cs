using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shared.Cqrs;

namespace Shared.Web;

[Route(BaseApiPath)]
[ApiController]
[ApiVersion("1.0")]
public abstract class BaseController : ControllerBase
{
    protected const string BaseApiPath = "api/v{version:apiVersion}";
    private IDispatcher _dispatcher;
    private IMapper _mapper;

    protected IDispatcher Dispatcher =>
        _dispatcher ??= HttpContext.RequestServices.GetService<IDispatcher>()!;

    protected IMapper Mapper =>
        _mapper ??= HttpContext.RequestServices.GetService<IMapper>()!;
}