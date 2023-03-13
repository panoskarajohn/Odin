using Shared.Cqrs.Queries;

namespace Event.Application.SportMatch.Features.GetTodayMatches;

public class GetTodayMatchesQuery : IQuery<IEnumerable<GetTodayMatchesResponse.MatchResponseDto>>
{
}