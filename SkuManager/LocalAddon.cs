using SkuManager.AddonManifests;

namespace SkuManager;
public record LocalAddon(AddonData AddonData, AddonVersion version, Uri? uri = null, int fileSize = 0);
