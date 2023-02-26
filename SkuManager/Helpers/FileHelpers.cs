using System.Security.Cryptography;
using System.Text;

using Ardalis.GuardClauses;


namespace SkuManager.Helpers;

/// <summary>
///  a small class to help with some file operations
/// </summary>
public static class FileHelpers
{
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
}
