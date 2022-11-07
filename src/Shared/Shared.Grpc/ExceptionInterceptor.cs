using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using Shared.Types.Exceptions;

namespace Shared.Grpc;


public class ExceptionInterceptor : Interceptor
{
    private readonly ILogger<ExceptionInterceptor> _logger;

    public ExceptionInterceptor(ILogger<ExceptionInterceptor> logger)
    {
        _logger = logger;
    }
    
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (OdinException e)
        {
            _logger.LogError(e, e.Message);
            throw new RpcException(new Status(StatusCode.Internal, e.Message));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw new RpcException(new Status(StatusCode.Internal, "An error occurred"));
        }
    }
}