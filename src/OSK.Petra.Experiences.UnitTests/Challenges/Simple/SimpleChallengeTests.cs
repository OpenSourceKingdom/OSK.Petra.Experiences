using Moq;
using OSK.Petra.Experiences.Challenges.Simple;
using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.UnitTests.Challenges.Simple;

public class SimpleChallengeTests
{
    #region Constructor Validations

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Constructor_InvalidName_ThrowsArgumentNullException(string? name)
    {
        // Arrange
        var id = 1;
        IEnumerable<IChallengeObjective> objectives = [];

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new SimpleChallenge(id, name!, []));
    }

    [Fact]
    public void Constructor_NullObjectives_ThrowsArgumentNullException()
    {
        // Arrange
        var id = 1;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new SimpleChallenge(id, "Test", null!));
    }

    [Fact]
    public void Constructor_Valid_ReturnsExpected()
    {
        // Arrange
        var id = 10;
        var name = "abc";
        var description = "Oh yeah";

        var mockObjective = Mock.Of<IChallengeObjective>();
        var objectives = new List<IChallengeObjective>() { mockObjective };

        var challenge = new SimpleChallenge(id, name, description, objectives);

        // Assert
        Assert.Equal(id, challenge.Id);
        Assert.Equal(name, challenge.Name);
        Assert.Equal(description, challenge.Description);

        Assert.Single(challenge.GetObjectives());
        Assert.Contains(mockObjective, challenge.GetObjectives());
    }

    #endregion

    #region GetObjectives

    [Fact]
    public void GetObjectives_ValidCollection_ReturnsArrayCopy()
    {
        // Arrange
        var challenge = new SimpleChallenge(1, "Test", "Description", []);

        // Act
        var objectives = challenge.GetObjectives();

        // Assert
        Assert.NotNull(objectives);
    }

    [Fact]
    public void GetObjectives_ContainsAllPassedObjectives_ReturnsCorrectCount()
    {
        // Arrange
        var challenge = new SimpleChallenge(1, "Test", [Mock.Of<IChallengeObjective>(), Mock.Of<IChallengeObjective>()]);

        // Act
        var objectives = challenge.GetObjectives();

        // Assert
        Assert.Equal(2, objectives.Count);
    }

    #endregion
}
