using Ardalis.GuardClauses;

using FluentAssertions;

using SkuManager.Extensions;

namespace SkuManager.Tests;

public class GuardClauseExtensionsTests
{
    [Fact]
    public void DoesNotExist_DoesNotThrowsException_WhenPathExists()
    {
        // Arrange
        var guardClause = Guard.Against;
        var input = "C:\\Windows";
        var parameterName = "path";

        // Act and Assert
        Action action = () => guardClause.DoesNotExist(input, parameterName);
        action.Should().NotThrow();       
    }

    [Fact]
    public void DoesNotExist_ThrowException_WhenPathDoesNotExist()
    {
        // Arrange
        var guardClause = Guard.Against;
        var input = "C:\\NonExistentFolder";
        var parameterName = "path";

        // Act and Assert
        Action action = () => guardClause.DoesNotExist(input, parameterName);

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage($"The path {input} does not exist (Parameter \'{parameterName}\')");
    }
}
