using Identity.Core.DTO;
using Identity.Core.Entities;

namespace Identity.Core.Queries.Handlers;

internal static class Extensions
{
    public static UserDto AsDto(this User user)
    {
        return user.Map<UserDto>();
    }

    public static UserDetailsDto AsDetailsDto(this User user)
    {
        var dto = user.Map<UserDetailsDto>();
        dto.Permissions = user.Role.Permissions;

        return dto;
    }

    private static T Map<T>(this User user) where T : UserDto, new()
    {
        return new()
        {
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role.Name,
            CreatedAt = user.CreatedAt
        };
    }
}