using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Common;
using Shared.DAL.Postgres.Internals;

namespace Shared.DAL.Postgres;

public static class Extensions
{
    public static IServiceCollection AddPostgres<T>(this IServiceCollection services,IConfiguration configuration) where T : DbContext
    {
        var section = configuration.GetSection("postgres");
        
        if (!section.Exists())
        {
            throw new InvalidOperationException("You should append the postgres configuration section to your appsettings.json file");
        }
        
        var options = section.BindOptions<PostgresOptions>();
        services.Configure<PostgresOptions>(section);
        services.AddDbContext<T>(x => x.UseNpgsql(options.ConnectionString));
        services.AddHostedService<DatabaseInitializer<T>>();
        services.AddHostedService<DataInitializer>();
        
        return services;
    }
    
    public static IServiceCollection AddDataInitializer<T>(this IServiceCollection services) where T : class, IDataInitializer
        => services.AddTransient<IDataInitializer, T>();
}