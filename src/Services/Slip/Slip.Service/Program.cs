using Grpc.Net.Client.Configuration;
using Shared.Cqrs;
using Shared.DAL.Postgres;
using Shared.DAL.Transactions;
using Shared.Grpc;
using Shared.Logging;
using Shared.MessageBroker.Cqrs;
using Shared.RabbitMq;
using Shared.Web;
using Slip.Service;
using Slip.Service.DAL;
using Slip.Service.Events.Externals;
using Slip.Service.Protos;
using Polly;
using RetryPolicy = Shared.Grpc.Polly.RetryPolicy;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext,services) =>
    {
        var configuration = hostContext.Configuration;
        services.AddHostedService<Worker>();

        services.AddGrpcClient<Event.EventClient>(options =>
        {
            options.Address = new Uri(configuration["urls:EventGrpcUrl"]);
        }).AddPolicyHandler(RetryPolicy.RetryFunc);

        services
            .AddHostApplication(configuration)
            .AddCqrs()
            .AddLogging()
            .AddPostgres<SlipContext>(configuration)
            .AddTransactionalDecorators()
            .AddRabbitMq(configuration);
    })
    .UseLogging()
    .Build();


host
    .UseRabbitMq()
    .SubscribeEvent<UserRequestedBetPlacement>();

host.Run();
