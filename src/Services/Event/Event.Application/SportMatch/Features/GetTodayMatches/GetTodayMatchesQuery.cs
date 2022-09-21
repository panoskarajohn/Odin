using Event.Application.SportMatch.Dtos;
using Shared.Cqrs.Queries;

namespace Event.Application.SportMatch.Features.GetTodayMatches;

public class GetTodayMatchesQuery : IQuery<IEnumerable<MatchResponseDto>>
{
}