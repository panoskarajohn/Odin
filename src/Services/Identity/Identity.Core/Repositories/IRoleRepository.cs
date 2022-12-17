using Identity.Core.Entities;

namespace Identity.Core.Repositories;

internal interface IRoleRepository
{
    Task<Role?> GetAsync(string name);
    Task<IReadOnlyList<Role>> GetAllAsync();
    Task AddAsync(Role role);
}