using Identity.Core.Entities;

namespace Identity.Core.Repositories;

internal interface IUserRepository
{
    Task<User?> GetAsync(string email);
    Task AddAsync(User user);
}