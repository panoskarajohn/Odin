
using Microsoft.Extensions.DependencyInjection;
using Shared.Cqrs.Commands;
using Shared.Cqrs.Events;
using Shared.Cqrs.Queries;
using Shared.Types.Attributes;

namespace Shared.Cqrs;

public static class Extensions
{
    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
        services.Scan(s => s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>))
                .WithoutAttribute(typeof(Decorator)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        return services;
    }
    
    private static IServiceCollection AddEvents(this IServiceCollection services)
    {
        services.AddSingleton<IEventDispatcher, EventDispatcher>();
        services.Scan(s =>
            s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>))
                    .WithoutAttribute(typeof(Decorator)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        return services;
    }
    
    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
        services.Scan(s =>
            s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>))
                    .WithoutAttribute(typeof(Decorator)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        return services;
    }

    public static IServiceCollection AddCqrs(this IServiceCollection services)
    {
        services.AddCommands()
            .AddEvents()
            .AddQueries();

        services.AddSingleton<IDispatcher, Dispatcher>();

        return services;
    }
}