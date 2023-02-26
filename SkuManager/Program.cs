using Towel;
using Octokit;
using Ardalis.GuardClauses;
using System.Text;
using System.Security.Cryptography;
using System.ComponentModel;
using SkuManager.Helpers;

namespace SkuManager;

internal static class Program
{
    private const string filename = "test.txt";

    private static async Task Main(string[] args)
    {
        
        var basicAuth = new Credentials("christopherpross", "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"); // NOTE: not real credentials
        var github    = new GitHubClient(new ProductHeaderValue("SkuManager"));
        github.Credentials = basicAuth; 
        var repo      = await github.Repository.Get("duugu", "sku");
        var release   = await github.Repository.Release.GetLatest(repo.Id);
        var tags      = await github.Repository.GetAllTags(repo.Id);
        var latestTag = tags.Single(x => x.Name == release.TagName);
        var comit     = await github.Repository.Commit.Get(repo.Id, latestTag.Commit.Sha);
        var tree      = await github.Git.Tree.GetRecursive(repo.Id, comit.Sha);
        var apiInfo   = github.GetLastApiInfo();
        //var remoteFile = tree.Tree.Single(x => x.Path.Contains(filename));
        var shaLocal = await FileHelpers.getGitFileShaAsync(filename);
        //bool compare = shaLocal.Equals(remoteFile.Sha, StringComparison.OrdinalIgnoreCase);    
        ConsoleHelper.PressToContinue();
    }

    public static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
}
