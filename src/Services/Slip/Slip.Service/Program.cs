using Shared.Cqrs;
using Shared.Logging;
using Shared.MessageBroker.Cqrs;
using Shared.RabbitMq;
using Shared.Web;
using Slip.Service;
using Slip.Service.Events.Externals;
using Slip.Service.Events.Handlers;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext,services) =>
    {
        var configuration = hostContext.Configuration;
        services.AddHostedService<Worker>();
        services
            .AddApplication(configuration)
            .AddCqrs()
            .AddLogging()
            .AddRabbitMq(configuration);
    })
    .UseLogging()
    .Build();

host
    .UseRabbitMq()
    .SubscribeEvent<UserRequestedBetPlacement>();

host.Run();
