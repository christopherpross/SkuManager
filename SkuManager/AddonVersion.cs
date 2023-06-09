using Ardalis.GuardClauses;

namespace SkuManager;
public class AddonVersion : IComparable<AddonVersion>
{
    public char? Prefix { get; set; }
    public int Major { get; }
    public int Minor { get; }
    public int Patch { get; }

    public AddonVersion(int major, int minor = 0, int patch = 0, char? prefix = null)
    {
        Major = major;
        Minor = minor;
        Patch = patch;
        Prefix = prefix;
    }

    public static AddonVersion Parse(string versionString)
    {
        if (string.IsNullOrEmpty(versionString))
            throw new ArgumentException($"Invalid version string {versionString}");

        char? prefix = null;
        int majorStartIndex = 0;

        if (char.IsLetter(versionString[0]))
        {
            prefix = versionString[0];
            majorStartIndex = 1;
        }

        string[] parts = versionString.Substring(majorStartIndex).Split('.');
        if (parts.Length < 1 || parts.Length > 3)
            throw new ArgumentException("Invalid version string");

        if (!int.TryParse(parts[0], out int major))
            throw new ArgumentException("Invalid version string");

        int minor = 0;
        int patch = 0;

        if (parts.Length >= 2)
        {
            if (!int.TryParse(parts[1], out minor))
                throw new ArgumentException("Invalid version string");

            if (parts.Length == 3)
            {
                if (!int.TryParse(parts[2], out patch))
                    throw new ArgumentException("Invalid version string");
            }
        }

        return new AddonVersion(major, minor, patch, prefix);
    }

    public int CompareTo(AddonVersion? other)
    {
        Guard.Against.Null(other, nameof(other));

        if (Major != other.Major)
            return Major.CompareTo(other.Major);

        if (Minor != other.Minor)
            return Minor.CompareTo(other.Minor);

        return Patch.CompareTo(other.Patch);
    }

    public static bool operator <(AddonVersion left, AddonVersion right)
    {
        if (left is null)
            return false;

        return left.CompareTo(right) < 0;
    }

    public static bool operator >(AddonVersion left, AddonVersion right)
    {
        if (left is null)
            return false;

        return left.CompareTo(right) > 0;
    }


    public static bool operator <=(AddonVersion left, AddonVersion right)
    {
        if (left is null)
            return false;

        return left.CompareTo(right) <= 0;
    }

    public static bool operator >=(AddonVersion left, AddonVersion right)
    {
        if (left is null)
            return false;

        return left.CompareTo(right) >= 0;
    }

    public static bool operator ==(AddonVersion left, AddonVersion right)
    {
        if (left is null)
            return right is null;

        return left.Equals(right);
    }

    public static bool operator !=(AddonVersion left, AddonVersion right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
    {
        if (obj is AddonVersion other)
        {
            return Major == other.Major && Minor == other.Minor && Patch == other.Patch;
        }

        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + Major.GetHashCode();
            hash = hash * 23 + Minor.GetHashCode();
            hash = hash * 23 + Patch.GetHashCode();
            return hash;
        }
    }

    public override string ToString()
    {
        if (Minor == 0 && Patch == 0)
        {
            return $"{((Prefix is null) ? string.Empty : Prefix)}{Major}";
        }
        else if (Patch == 0)
        {
            return $"{((Prefix is null) ? string.Empty : Prefix)}{Major}.{Minor}";
        }
        else
        {
            return $"{((Prefix is null) ? string.Empty : Prefix)}{Major}.{Minor}.{Patch}";
        }
    }
}
