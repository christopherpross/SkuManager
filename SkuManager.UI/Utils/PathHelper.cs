using System.IO;
using System.Diagnostics;
using System;

namespace SkuManager.UI.Utils;
public static class PathHelper
{
    public static string? RootDirectory
    {
        get
        {
            return Path.GetDirectoryName(Environment.ProcessPath);
        }
    }

    public static string LogDirectory
    {
        get
        {
            return Path.Combine(RootDirectory ?? AppDomain.CurrentDomain.BaseDirectory, "logs");
        }
    }

    public static string ManifestDirectory
    {
        get
        {
            return Path.Combine(RootDirectory ?? AppDomain.CurrentDomain.BaseDirectory, "manifests");
        }
    }
}
