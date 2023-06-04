namespace SkuManager.AddonManifests;
public class AddonManifest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<AddonData>? Addons { get; set; }
}
