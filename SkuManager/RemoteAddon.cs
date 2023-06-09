using SkuManager.AddonManifests;

namespace SkuManager;
public record RemoteAddon(AddonData AddonData, AddonVersion Version, Uri Uri , int FileSize, string ReleaseNotes);
