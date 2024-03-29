﻿using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Shared.DAL.Postgres.Internals;

public sealed class DatabaseInitializer<T> : IHostedService where T : DbContext
{
    private readonly IServiceProvider _serviceProvider;
    public DatabaseInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var dbContext = (DbContext)scope.ServiceProvider.GetRequiredService<T>();
        await dbContext.Database.MigrateAsync(cancellationToken);

    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

}