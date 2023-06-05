using FluentAssertions;

using Xunit;

namespace SkuManager.Tests;

public class AddonVersionTests
{
    [Theory]
    [InlineData("r34.17", 'r', 34, 17, 0)]
    [InlineData("r34.16", 'r', 34, 16, 0)]
    [InlineData("r34", 'r', 34, 0, 0)]
    [InlineData("r31.8", 'r', 31, 8, 0)]
    [InlineData("34.10", null, 34, 10, 0)]
    [InlineData("1.0.0", null, 1, 0, 0)]
    [InlineData("1", null, 1, 0, 0)]
    public void Parse_ValidVersionString_ReturnsExpectedVersion(string versionString, char? expectedPrefix, int expectedMajor, int expectedMinor, int expectedPatch)
    {
        // Act
        AddonVersion version = AddonVersion.Parse(versionString);

        // Assert
        version.Prefix.Should().Be(expectedPrefix);
        version.Major.Should().Be(expectedMajor);
        version.Minor.Should().Be(expectedMinor);
        version.Patch.Should().Be(expectedPatch);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("")]
    [InlineData("r34.17.25.12")]
    public void Parse_InvalidVersionString_ThrowsArgumentException(string versionString)
    {
        // Arrange & Act & Assert
        versionString.Invoking(vs => AddonVersion.Parse(vs)).Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("r34.17", "r34.16", false)]
    [InlineData("r34.16", "r34.17", false)]
    [InlineData("r34.17", "r34.17", true)]
    [InlineData("r34", "r31.8", false)]
    [InlineData("r34", "r35", false)]
    [InlineData("34.10", "1.0.0", false)]
    [InlineData("1.0.0", "1", true)]
    [InlineData("1.0.0", "1.0.0", true)]
    public void ComparisonEqual_Operators_CompareVersionsCorrectly(string versionString1, string versionString2, bool expectedComparisonResult)
    {
        // Arrange
        AddonVersion version1 = AddonVersion.Parse(versionString1);
        AddonVersion version2 = AddonVersion.Parse(versionString2);

        // Act & Assert
        (version1 == version2).Should().Be(expectedComparisonResult);
        (version1 != version2).Should().Be(!expectedComparisonResult);
    }

    [Theory]
    [InlineData("r34.17", "r34.16", false)]
    [InlineData("r34.16", "r34.17", true)]
    [InlineData("r34.17", "r34.17", false)]
    [InlineData("r34", "r31.8", false)]
    [InlineData("r34", "r35", true)]
    [InlineData("34.10", "1.0.0", false)]
    [InlineData("1.0.0", "1", false)]
    [InlineData("1.0.0", "1.0.0", false)]
    public void ComparisonLess_Operators_CompareVersionsCorrectly(string versionString1, string versionString2, bool expectedComparisonResult)
    {
        // Arrange
        AddonVersion version1 = AddonVersion.Parse(versionString1);
        AddonVersion version2 = AddonVersion.Parse(versionString2);

        // Act & Assert
        (version1 < version2).Should().Be(expectedComparisonResult);
    }

    [Theory]
    [InlineData("r34.17", "r34.16", false)]
    [InlineData("r34.16", "r34.17", true)]
    [InlineData("r34.17", "r34.17", true)]
    [InlineData("r34", "r31.8", false)]
    [InlineData("r34", "r35", true)]
    [InlineData("34.10", "1.0.0", false)]
    [InlineData("1.0.0", "1", true)]
    [InlineData("1.0.0", "1.0.0", true)]
    public void ComparisonLessOrEqual_Operators_CompareVersionsCorrectly(string versionString1, string versionString2, bool expectedComparisonResult)
    {
        // Arrange
        AddonVersion version1 = AddonVersion.Parse(versionString1);
        AddonVersion version2 = AddonVersion.Parse(versionString2);

        // Act & Assert
        (version1 <= version2).Should().Be(expectedComparisonResult);
    }

    [Theory]
    [InlineData("r34.17", "r34.16", true)]
    [InlineData("r34.16", "r34.17", false)]
    [InlineData("r34.17", "r34.17", false)]
    [InlineData("r34", "r31.8", true)]
    [InlineData("r34", "r35", false)]
    [InlineData("34.10", "1.0.0", true)]
    [InlineData("1.0.0", "1", false)]
    [InlineData("1.0.0", "1.0.0", false)]
    public void ComparisonGreater_Operators_CompareVersionsCorrectly(string versionString1, string versionString2, bool expectedComparisonResult)
    {
        // Arrange
        AddonVersion version1 = AddonVersion.Parse(versionString1);
        AddonVersion version2 = AddonVersion.Parse(versionString2);

        // Act & Assert
        (version1 > version2).Should().Be(expectedComparisonResult);
    }

    [Theory]
    [InlineData("r34.17", "r34.16", true)]
    [InlineData("r34.16", "r34.17", false)]
    [InlineData("r34.17", "r34.17", true)]
    [InlineData("r34", "r31.8", true)]
    [InlineData("r34", "r35", false)]
    [InlineData("34.10", "1.0.0", true)]
    [InlineData("1.0.0", "1", true)]
    [InlineData("1.0.0", "1.0.0", true)]
    public void ComparisonGreaterOrEqual_Operators_CompareVersionsCorrectly(string versionString1, string versionString2, bool expectedComparisonResult)
    {
        // Arrange
        AddonVersion version1 = AddonVersion.Parse(versionString1);
        AddonVersion version2 = AddonVersion.Parse(versionString2);

        // Act & Assert
        (version1 >= version2).Should().Be(expectedComparisonResult);
    }



}

