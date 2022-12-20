using Identity.Core.DTO;
using Shared.Cqrs.Queries;

namespace Identity.Core.Queries;

public class GetUser : IQuery<UserDetailsDto?>
{
    public Guid UserId { get; set; }
}