using Event.Application.Match.Dtos;
using Event.Infrastructure.Mongo;

namespace Event.Application.Match.Mapping;

public static class MatchDocumentToResponse
{
    public static MatchResponseDto ToResponse(this MatchDocument document)
    {
        return new MatchResponseDto(document.Id,
            document.Category,
            document.StartingTime,
            document.MatchName);
    }
}