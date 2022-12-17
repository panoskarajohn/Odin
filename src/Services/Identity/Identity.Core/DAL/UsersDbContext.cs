using Identity.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Core.DAL;

public class UsersDbContext : DbContext
{
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}