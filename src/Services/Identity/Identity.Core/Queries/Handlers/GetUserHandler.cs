using Identity.Core.DAL;
using Identity.Core.DTO;
using Microsoft.EntityFrameworkCore;
using Shared.Cqrs.Queries;

namespace Identity.Core.Queries.Handlers;

public class GetUserHandler : IQueryHandler<GetUser, UserDetailsDto?>
{
    private readonly UsersDbContext _dbContext;

    public GetUserHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserDetailsDto?> HandleAsync(GetUser query, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .Include(x => x.Role)
            .SingleOrDefaultAsync(x => x.Id == query.UserId, cancellationToken);

        return user?.AsDetailsDto();
    }
}