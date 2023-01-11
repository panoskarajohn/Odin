using Microsoft.EntityFrameworkCore;

namespace Slip.Service.DAL;

public class SlipContext : DbContext
{
    public DbSet<Domain.Slip> Slips { get; set; } = null!;
    public DbSet<Domain.Bet> Bets { get; set; } = null!;
    public DbSet<Domain.BetSelection> Selections { get; set; } = null!;
    
    public SlipContext(DbContextOptions<SlipContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}