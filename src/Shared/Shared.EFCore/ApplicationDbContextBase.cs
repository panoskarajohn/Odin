
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.Web.Context;

namespace Shared.EFCore;

public class ApplicationDbContextBase : DbContext, IDbContext
{
    private readonly IContext _context;
    public const string DefaultSchema = "dbo";
    private IDbContextTransaction _currentTransaction;
    
    public ApplicationDbContextBase(DbContextOptions dbContextOptions,IContext context) : base(dbContextOptions)
    {
        _context = context;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ref: https://github.com/pdevito3/MessageBusTestingInMemHarness/blob/main/RecipeManagement/src/RecipeManagement/Databases/RecipesDbContext.cs
        base.OnModelCreating(modelBuilder);
    }
    
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null) 
            return;

        _currentTransaction = await Database.BeginTransactionAsync(cancellationToken);
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}