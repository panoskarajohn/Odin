using System.Net;
using Grpc.Core;
using Polly;

namespace Shared.Grpc.Polly;

public static class RetryPolicy
{
    private static readonly HttpStatusCode[] _serverErrors =
    {
        HttpStatusCode.BadGateway,
        HttpStatusCode.GatewayTimeout,
        HttpStatusCode.ServiceUnavailable,
        HttpStatusCode.InternalServerError,
        HttpStatusCode.TooManyRequests,
        HttpStatusCode.RequestTimeout,
    };

    private static StatusCode[] _gRpcErrors =
    {
        StatusCode.DeadlineExceeded,
        StatusCode.Internal,
        StatusCode.NotFound,
        StatusCode.ResourceExhausted,
        StatusCode.Unavailable,
        StatusCode.Unknown,
    };

    public static Func<HttpRequestMessage, IAsyncPolicy<HttpResponseMessage>> RetryFunc => request =>
    {
        return Policy.HandleResult<HttpResponseMessage>(r =>
            {
                var hasHeader = r.Headers.TryGetValues("grpc-status", out var values);
                if (hasHeader)
                    return values.FirstOrDefault() != "0";
                return _serverErrors.Contains(r.StatusCode);
            })
            .WaitAndRetryAsync(3, input => TimeSpan.FromSeconds(3 + input), (result, timeSpan, retryCount, context) =>
            {
                var grpcStatus =
                    (StatusCode) int.Parse(result.Result.Headers.GetValues("grpc-status").FirstOrDefault());
                Console.WriteLine($"Request failed with {grpcStatus}. Retry.");
            });
    };
}