using SkuManager.AddonManifests;

namespace SkuManager.Update;
public record GithubAddonUpdaterOptions(string? WoWPathInterface, string? WoWPathAddons, string? WoWPathWTF, string? WoWMenuPath, AddonManifest? AddonManifest);
