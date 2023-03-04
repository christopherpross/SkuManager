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
}
