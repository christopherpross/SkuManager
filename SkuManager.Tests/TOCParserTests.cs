using Xunit;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

namespace SkuManager.Tests;

public class TOCParserTests
{
    [Theory]
    [InlineData("90001", "Mein Addon", "Mein Name", "1.0", new[] { "MeineHauptdatei.lua", "WeitereDatei.lua" })]
    [InlineData("80000", "Test Addon", "Test Autor", "2.5", new[] { "Haupt.lua", "Helper.lua", "Config.lua" })]
    public void ParseTOCFile_ValidTOCFile_ParsesSuccessfully(string expectedInterface, string expectedTitle, string expectedAuthor, string expectedVersion, string[] expectedLuaFiles)
    {
        // Arrange
        string tocContent = $"## Interface: {expectedInterface}\n" +
                            $"## Title: {expectedTitle}\n" +
                            $"## Author: {expectedAuthor}\n" +
                            $"## Version: {expectedVersion}\n" +
                            string.Join("\n", expectedLuaFiles);

        var fileSystem = new MockFileSystem();
        var filePath = "path/to/toc-file.toc";
        var directoryPath = "path/to";

        fileSystem.Directory.CreateDirectory(directoryPath);
        fileSystem.File.WriteAllText(filePath, tocContent);

        TOCParser parser = new TOCParser(filePath, fileSystem);

        // Act & Assert
        Assert.Equal(expectedInterface, parser.Interface);
        Assert.Equal(expectedTitle, parser.Title);
        Assert.Equal(expectedAuthor, parser.Author);
        Assert.Equal(expectedVersion, parser.Version);
        Assert.Equal(expectedLuaFiles.Length, parser.LuaFiles.Count);
        Assert.Equal(expectedLuaFiles, parser.LuaFiles);
    }
}
