using System.IO;
using System.Diagnostics;
using System;
using SkuManager.UI.Properties;

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
            return Path.Combine(RootDirectory ?? AppDomain.CurrentDomain.BaseDirectory, "AddonManifests");
        }
    }

    public static string WoWPathAddonFolder
    {
        get
        {
            return Path.Combine(Settings.Default.WoWInterfaceFolderPath, "AddOns");
        }
    }

    public static string WoWPathWTFFolder
    {
        get
        {
            return Path.Join(Path.GetFullPath(Path.Combine(Settings.Default.WoWInterfaceFolderPath, @"..\")), "WTF");
        }
    }
}
