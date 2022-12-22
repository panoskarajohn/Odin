namespace Identity.Core.DTO;

public class UserDetailsDto : UserDto
{
    public IEnumerable<string>? Permissions { get; set; }
}