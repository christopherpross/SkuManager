namespace SkuManager.AddonManifest;
public class AddonData
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? GithubOwner { get; set; }
    public string? GithubRepo { get; set; }
    public bool NeedsExtraPath { get; set; } = false;
}
