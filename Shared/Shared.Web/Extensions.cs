using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Common;
using Shared.Types.Interfaces;
using Shared.Web.Context;
using Shared.Web.ErrorHandling;
using Shared.Web.Extension;
using Shared.Web.Options;

namespace Shared.Web;

public static class Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static IServiceCollection AddContext(this IServiceCollection services)
    {
        services.AddSingleton<ContextAccessor>();
        services.AddTransient(sp => sp.GetRequiredService<ContextAccessor>().Context);
            
        return services;
    }
    
    public static IServiceCollection AddErrorHandling(this IServiceCollection services)
        => services
            .AddScoped<ErrorHandlerMiddleware>()
            .AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>()
            .AddSingleton<IExceptionCompositionRoot, ExceptionCompositionRoot>();

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    private static IApplicationBuilder UseContext(this IApplicationBuilder app)
    {
        app.Use((ctx, next) =>
        {
            ctx.RequestServices.GetRequiredService<ContextAccessor>().Context = new Context.Context(ctx);;
            return next();
        });
        return app;
    }
    
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var appOptions = configuration.GetOptions<AppOptions>("app");
        services.AddSingleton(appOptions);
        services.AddContext();
        
        var version = appOptions.DisplayVersion ? $" {appOptions.Version}" : string.Empty;
        Console.WriteLine(Figgle.FiggleFonts.Doom.Render($"{appOptions.Name} v.{version}"));
        
        return services;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseApplication(this IApplicationBuilder app)
    {
        app.UseContext();
        app.UseCorrelationId();
        
        using var scope = app.ApplicationServices.CreateScope();
        var initializer = scope.ServiceProvider.GetServices<IInitializer>();
        
        Task.Run(() =>
        {
            foreach (var initializer1 in initializer)
            {
                initializer1.InitAsync().GetAwaiter().GetResult();
            }
        });
    
        return app;
    }
    
    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
        => app.UseMiddleware<ErrorHandlerMiddleware>();
}

