using Event.Application.Match.Dtos;
using Event.Infrastructure.Mongo;

namespace Event.Application.Match.Mapping;

public static class MatchDocumentToResponse
{
    public static MatchesResponseDto ToResponse(this MatchDocument document)
    {
        return new MatchesResponseDto(document.Id, 
            document.Category, 
            document.StartingTime, 
            document.MatchName);
    }
}