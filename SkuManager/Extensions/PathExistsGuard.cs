using System.IO;

namespace Ardalis.GuardClauses;

public static class PathExistsGuard
{
    public static void DoesNotExist(this IGuardClause guardClause, string input, string parameterName)
    {
        if (!FileOrDirectoryExists(input))
            throw new ArgumentException(string.Format("The path {0} does not exist", input), parameterName);
    }

    internal static bool FileOrDirectoryExists(string path)
    {
        return (Directory.Exists(path) || File.Exists(path));
    }
}
