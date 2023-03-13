namespace Identity.Core.Entities;

public class Role
{
    public const string User = "user";
    public const string Admin = "admin";
    public string Name { get; set; } = string.Empty;
    public IEnumerable<string> Permissions { get; set; } = new List<string>();
    public IEnumerable<User> Users { get; set; } = new List<User>();

    public static string Default => User;
}