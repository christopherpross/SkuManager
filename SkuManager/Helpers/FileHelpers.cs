using System.Security.Cryptography;
using System.Text;

using Ardalis.GuardClauses;

using SkuManager.Extensions;

namespace SkuManager.Helpers;

/// <summary>
///  a small class to help with some file operations
/// </summary>
public static class FileHelpers
{
    private static readonly string[] textFileExtensions =
    {
        ".txt", // text-files
        ".toc", // toc-format from WoW-Addons
        ".lua", // addon source files
        ".md", // markdown files
        ".xml", // xml files
        ".json" // json files
    };

    /// <summary>
    /// calculate the git blob sha for a given file
    /// </summary>
    /// <param name="filename">path to the file to generate the sha from</param>
    /// <returns>the sha in hex format</returns>
    /// <remarks>
    /// please note that the git blob sha contains upper-case letters, the method <see cref="Convert.ToHexString(byte[])"/> returns lower-case letters.
    /// </remarks>
    public static async Task<string> getGitFileShaAsync(string filename)
    {
        Guard.Against.NullOrWhiteSpace(filename, nameof(filename));
        Guard.Against.DoesNotExist(filename, nameof(filename));
        
        var fileContent = await File.ReadAllBytesAsync(filename);
        byte[] b1 = Encoding.ASCII.GetBytes(string.Format("blob {0}\0", fileContent.Length));
        byte[] toHash = b1.Concat(fileContent).ToArray();
        byte[] hash = SHA1.Create().ComputeHash(toHash);
        return Convert.ToHexString(hash);
    }

    /// <summary>
    /// checks whether the given file is a text file
    /// </summary>
    /// <param name="filename">name or path of the file
    /// <returns>true if the given file contains plain text, false if not.
    public  static bool IsTextFile(string filename)
    {
        Guard.Against.NullOrWhiteSpace(filename, nameof(filename)); 

        foreach (string extension in textFileExtensions)
        {
            if (filename.EndsWith(extension))
                return true;
        }
        return false;
    }
}
