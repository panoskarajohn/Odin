namespace Shared.Web.Options;

/// <summary>
///     This should Bind to an app section in appsettings.json
/// </summary>
public class AppOptions
{
    public string Name { get; set; } = string.Empty;
    public string Service { get; set; } = string.Empty;
    public string Instance { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public bool DisplayBanner { get; set; } = true;
    public bool DisplayVersion { get; set; } = true;
}