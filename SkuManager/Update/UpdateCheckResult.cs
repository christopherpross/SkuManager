using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkuManager.Update;
public record UpdateCheckResult(string? Name, AddonVersion? LocalVersion, AddonVersion? RemoteVersion, string? ReleaseNotes)
{
    public override string? ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("{0} - {1} -> {2}", Name, LocalVersion, RemoteVersion);
        sb.AppendLine();

        if (!string.IsNullOrWhiteSpace(ReleaseNotes))
        {
            sb.AppendLine("Changelog:");
            sb.AppendLine(ReleaseNotes);
        }
        return sb.ToString();
    }
}
