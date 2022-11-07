using Event.Application.SportMatch.Dtos;
using Shared.Cqrs.Queries;

namespace Event.Application.SportMatch.Features.GetMatch;

public sealed record GetMatchQuery(long Id) : IQuery<MatchResponseDto>;