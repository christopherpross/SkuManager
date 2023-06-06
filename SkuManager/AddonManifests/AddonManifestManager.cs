using System.Text.Json;

using Ardalis.GuardClauses;

using Microsoft.Extensions.Logging;

using SkuManager.Extensions;

namespace SkuManager.AddonManifests;

public class AddonManifestManager
{
    private readonly static string manifestFileExtension = ".addon-manifest";
    private readonly ILogger<AddonManifestManager> logger;

    public AddonManifestManager(ILogger<AddonManifestManager> logger)
    {
        this.logger = logger;
    }

    public List<AddonManifest> LoadAddonManifests(string directoryPath)
    {
        Guard.Against.NullOrWhiteSpace(directoryPath, nameof(directoryPath));
        Guard.Against.DoesNotExist(directoryPath, nameof(directoryPath));
        List<AddonManifest> manifests = new List<AddonManifest>();

        try
        {
            string[] manifestFiles = Directory.GetFiles(directoryPath, "*" + manifestFileExtension);
            if (manifestFiles.Length == 0)
            {
                logger.LogWarning("No manifest files found in {path}", directoryPath);
                return manifests;
            }

            manifests = manifestFiles
                .Select(LoadAddonManifestFromFile)
                .Where(manifest => manifest != null)
                .Select(manifest => manifest!)
                .ToList();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load addon manifests from path {path}", directoryPath);
        }

        return manifests;
    }

    public AddonManifest? GetAddonManifestByName(string name, string directoryPath)
    {
        Guard.Against.NullOrWhiteSpace(directoryPath, nameof(directoryPath));
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Guard.Against.DoesNotExist(directoryPath, nameof(directoryPath));

        try
        {
            var manifest = LoadAddonManifests(directoryPath).FirstOrDefault(m => m.Name == name);

            if (manifest != null)
            {
                return manifest;
            }

            logger.LogWarning("Addon manifest for {addonName} not found.", name);
        }
        catch (Exception ex)
        {
                    logger.LogError(ex, "Failed to get addon manifest by name {addonName}", name);
        }
        return null;
    }

    private AddonManifest? LoadAddonManifestFromFile(string filePath)
    {
        Guard.Against.NullOrWhiteSpace(filePath, nameof(filePath));
        Guard.Against.DoesNotExist(filePath, nameof(filePath));

        try
        {
            string jsonString = File.ReadAllText(filePath);
            AddonManifest? manifest = JsonSerializer.Deserialize<AddonManifest?>(jsonString);

            return manifest;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load addon manifest from file {path}.", filePath);
            return null;
        }
    }
}
