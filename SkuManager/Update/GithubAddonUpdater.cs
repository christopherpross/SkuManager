using Octokit;
using Microsoft.Extensions.Logging;
using SkuManager.AddonManifests;
using Ardalis.GuardClauses;
using System.IO.Abstractions;
using System.Text;

namespace SkuManager.Update;
public class GithubAddonUpdater
{
    private readonly ILogger<GithubAddonUpdater> logger;
    private readonly GitHubClient gitHubClient;
    private readonly GithubAddonUpdaterOptions options;
    private readonly AddonManifest addonManifest;

    public GithubAddonUpdater(ILogger<GithubAddonUpdater> logger, GitHubClient gitHubClient, GithubAddonUpdaterOptions options)
    {
        this.options = options ?? throw new ArgumentNullException(nameof(options));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.gitHubClient = gitHubClient ?? throw new ArgumentNullException(nameof(gitHubClient));
        this.addonManifest = options.AddonManifest ?? throw new ArgumentNullException(nameof(addonManifest));
    }

    public LocalAddon GetLocalAddon(AddonData addonData)
    {
        Guard.Against.Null(addonData, nameof(addonData));

        string addonDirectory;
        if (!string.IsNullOrEmpty(addonData.Name) && addonData.Name.Equals("wow_menu", StringComparison.OrdinalIgnoreCase))
        {
            addonDirectory = options.WoWMenuPath ?? string.Empty;
        } else
        {
            addonDirectory = Path.Join(options.WoWPathAddons, addonData.Name);
        }
        
        if (!Directory.Exists(addonDirectory))
        {
            throw new Exception($"The addon {addonData.Name} could not be found in the WoW addon directory {options.WoWPathAddons}");
        }

        var addonTocFileName = string.Empty;
        if (string.IsNullOrWhiteSpace(addonData.TocFileName))
        {
            addonTocFileName = addonData.Name + ".toc";
        }
        else
        {
            addonTocFileName = addonData.TocFileName;
        }

        var addonTocPath = Path.Join(addonDirectory, addonTocFileName);

        if (!File.Exists(addonTocPath))
        {
            throw new Exception($"The addon-toc-file {addonTocPath} does not exist.");
        }
        logger.LogInformation("Parsing version of {tocpath}", addonTocPath);
        TOCParser TOCParser = new TOCParser(addonTocPath, new FileSystem());

        return new LocalAddon(addonData, AddonVersion.Parse(TOCParser.Version), new Uri(addonDirectory));
    }

    public async Task<RemoteAddon> GetRemoteAddon(AddonData addonData)
    {
        Guard.Against.Null(addonData, nameof(addonData));

        Release release;

        try
        {
            release = await gitHubClient.Repository.Release.GetLatest(addonData.GithubOwner, addonData.GithubRepo);
        }
        catch (ApiException ex)
        {
            logger.LogError(ex, "Got api error while trying get latest release for {addon}: {message}", addonData.Name, ex.ApiError.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while trying to get latest release for addon {addon}", addonData.Name);
            throw;
        }

        var ratelimit = gitHubClient.GetLastApiInfo().RateLimit;
        logger.LogInformation("requests left after get latest release for {addon}: {requestsLeft}", addonData.Name, ratelimit.Remaining);

        if (release.Assets.Count <= 0)
        {
            logger.LogError("no assets found for {addon}", addonData.Name);
            throw new Exception($"No assets found for {addonData.Name}");
        }

        ReleaseAsset? zipAsset = release.Assets.FirstOrDefault(a => a.Name.EndsWith(".zip"));
        if (zipAsset is null)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var asset in release.Assets) { sb.Append(asset.Name); }

            logger.LogError("Could not find zip asset for {addon}, avaiable assets are: {assets}", addonData.Name, sb.ToString());
            throw new Exception($"Could not find zip asset for {addonData.Name}, avaiable assets are: {sb.ToString()}");
        }

        logger.LogInformation("parsing version for addon {addon}", addonData.Name);

        return new RemoteAddon(addonData,
                               AddonVersion.Parse(release.TagName),
                               new Uri(zipAsset.Url),
                               zipAsset.Size,
                               release.Body);
    }


    public async Task<IEnumerable<UpdateCheckResult>> CheckForUpdates(IProgress<int> progress, CancellationToken cancellationToken)
    {
        var result = new List<UpdateCheckResult>();
        if (addonManifest is null)
        {
            throw new InvalidOperationException("the addonmanifest can not be null");
        }
        if (addonManifest.Addons is null)
        {
            throw new InvalidOperationException("The addons in the addonmanifest can not be null");
        }

        int totalAddons = addonManifest.Addons.Count;

        for (int i = 0; i < totalAddons; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            AddonData addon = addonManifest.Addons[i];
            var localAddon = GetLocalAddon(addon);
            var remoteAddon = await GetRemoteAddon(addon).ConfigureAwait(false);
            result.Add(new UpdateCheckResult(localAddon, remoteAddon));
            int progressPercentage = (int)((i + 1) / (double)totalAddons * 100);
            if (progressPercentage <= 0) progressPercentage = 1;
            progress?.Report(progressPercentage);
        }

        return result.Where(a => a.UpdateAvaiable);
    }
}
