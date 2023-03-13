using Identity.Core.Entities;
using Identity.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Identity.Core.DAL.Repositories;

internal class RoleRepository : IRoleRepository
{
    private readonly UsersDbContext _context;
    private readonly DbSet<Role> _roles;

    public RoleRepository(UsersDbContext context)
    {
        _context = context;
        _roles = context.Roles;
    }

    public Task<Role?> GetAsync(string name)
    {
        return _roles.SingleOrDefaultAsync(x => x.Name == name);
    }

    public async Task<IReadOnlyList<Role>> GetAllAsync()
    {
        return await _roles.ToListAsync();
    }

    public async Task AddAsync(Role role)
    {
        await _roles.AddAsync(role);
        await _context.SaveChangesAsync();
    }
}