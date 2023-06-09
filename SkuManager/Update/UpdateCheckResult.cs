using System.Text;

namespace SkuManager.Update;
public class UpdateCheckResult
{
    public LocalAddon? localAddon { get; set; }
    public RemoteAddon? remoteAddon { get; set; }

    public string? Name => localAddon?.AddonData.Name;
    public AddonVersion? LocalVersion => localAddon?.version;
    public AddonVersion? RemoteVersion => remoteAddon?.Version;

    public UpdateCheckResult(LocalAddon? localAddon, RemoteAddon? remoteAddon)
    {
        if (localAddon?.AddonData != remoteAddon?.AddonData)
            throw new ArgumentException("LocalAddon and RemoteAddon have different data.");

        this.localAddon = localAddon;
        this.remoteAddon = remoteAddon;
    }

    public override string? ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("{0} - {1} -> {2}", Name, LocalVersion, RemoteVersion);
        sb.AppendLine();

        if (!string.IsNullOrWhiteSpace(remoteAddon?.ReleaseNotes))
        {
            sb.AppendLine("Changelog:");
            sb.AppendLine(remoteAddon?.ReleaseNotes);
        }
        return sb.ToString();
    }
}
