using Microsoft.EntityFrameworkCore;
using Slip.Service.Domain;

namespace Slip.Service.DAL;

public class SlipContext : DbContext
{
    public SlipContext(DbContextOptions<SlipContext> options) : base(options)
    {
    }

    public DbSet<Domain.Slip> Slips { get; set; } = null!;
    public DbSet<Bet> Bets { get; set; } = null!;
    public DbSet<BetSelection> Selections { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}