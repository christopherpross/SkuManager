namespace SkuManager.AddonManifests;
public class AddonData
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? GithubOwner { get; set; }
    public string? GithubRepo { get; set; }
    public bool NeedsExtraPath { get; set; } = false;
    public string TocFileName { get; set; } = string.Empty;
}
