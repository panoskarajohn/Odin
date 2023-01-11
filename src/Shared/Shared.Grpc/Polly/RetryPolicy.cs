using System.Net;
using Grpc.Core;
using Polly;


namespace Shared.Grpc.Polly;

public static class RetryPolicy
{
    static HttpStatusCode[] _serverErrors = { 
        HttpStatusCode.BadGateway, 
        HttpStatusCode.GatewayTimeout, 
        HttpStatusCode.ServiceUnavailable, 
        HttpStatusCode.InternalServerError, 
        HttpStatusCode.TooManyRequests, 
        HttpStatusCode.RequestTimeout 
    };

    static StatusCode[] _gRpcErrors = {
        StatusCode.DeadlineExceeded,
        StatusCode.Internal,
        StatusCode.NotFound,
        StatusCode.ResourceExhausted,
        StatusCode.Unavailable,
        StatusCode.Unknown
    };

    public static Func<HttpRequestMessage, IAsyncPolicy<HttpResponseMessage>> RetryFunc => (request) =>
    {
        return Policy.HandleResult<HttpResponseMessage>(r => r.Headers.GetValues("grpc-status").FirstOrDefault() != "0")
            .WaitAndRetryAsync(3, (input) => TimeSpan.FromSeconds(3 + input), (result, timeSpan, retryCount, context) =>
            {
                var grpcStatus =
                    (StatusCode) int.Parse(result.Result.Headers.GetValues("grpc-status").FirstOrDefault());
                Console.WriteLine($"Request failed with {grpcStatus}. Retry.");
            });
    };
}