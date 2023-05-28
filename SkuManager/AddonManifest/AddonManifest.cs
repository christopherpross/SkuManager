namespace SkuManager.AddonManifest;
public class AddonManifest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<AddonData>? addons { get; set; }
}
