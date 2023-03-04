using System.Text;

using FluentAssertions;

using SkuManager.Helpers;

namespace SkuManager.Tests;

public class FileHelpersTests
{
    [Fact]
    public async Task GetGitFileShaAsync_ReturnsValidSha()
    {
        // Arrange
        var filename = "testfile.txt";
        var fileContent = Encoding.UTF8.GetBytes("test content");
        await File.WriteAllBytesAsync(filename, fileContent);
        var expectedSha = "08CF6101416F0CE0DDA3C80E627F333854C4085C";

        // Act
        var actualSha = await FileHelpers.getGitFileShaAsync(filename);

        // Assert
        actualSha.Should().Be(expectedSha);

        // Cleanup
        File.Delete(filename);
    }

    [Fact]
    public async Task GetGitFileShaAsync_ThrowsExceptionWhenFileNotFound()
    {
        // Arrange
        var filename = "nonexistentfile.txt";

        // Act and Assert
        Func<Task> act = async () => await FileHelpers.getGitFileShaAsync(filename);
        await act.Should().ThrowAsync<ArgumentException>().ConfigureAwait(false);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetGitFileShaAsync_ThrowsExceptionWhenFilenameIsNullOrEmpty(string filename)
    {
        // Act and Assert
        Func<Task> action = async () => await FileHelpers.getGitFileShaAsync(filename);
        await action.Should().ThrowAsync<ArgumentException>().ConfigureAwait(false);
    }

    [Fact]
    public void IsTextFile_ShouldReturnTrue_WhenFilenameEndsWithTextFileExtension()
    {
        // Arrange
        string filename = "example.txt";

        // Act
        bool result = FileHelpers.IsTextFile(filename);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsTextFile_ShouldReturnFalse_WhenFilenameDoesNotEndWithTextFileExtension()
    {
        // Arrange
        string filename = "example.jpg";

        // Act
        bool result = FileHelpers.IsTextFile(filename);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void IsTextFile_ShouldThrowException_WhenFilenameIsNullOrWhitespace(string filename)
    {
        // Act & Assert
        /*
        Assert.Throws<GuardClauseException>(() => FileHelpers.IsTextFile(filename))
            .ParamName.Should().Be(nameof(filename));
        */

        Action act = () => FileHelpers.IsTextFile(filename);
        act.Should().Throw<ArgumentException>()
            .WithParameterName(nameof(filename));
    }
}
