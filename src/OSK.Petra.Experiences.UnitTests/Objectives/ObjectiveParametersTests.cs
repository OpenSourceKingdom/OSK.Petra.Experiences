using OSK.Petra.Experiences.Models;
using OSK.Petra.Experiences.Objectives;

namespace OSK.Petra.Experiences.UnitTests.Objectives;

public class ObjectiveParametersTests
{
    #region Validate

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Validate_InvalidName_ThrowsArgumentNullException(string? name)
    {
        // Arrange
        var parameters = new ObjectiveParameters()
        {
            Name = name!
        };

        // Act/Assert
        Assert.Throws<ArgumentNullException>(() => parameters.Validate());
    }

    [Fact]
    public void Validate_PreventInvalidResolutionAndExpectation_ThrowsInvalidOperationException()
    {
        // Arrange
        var parameters = new ObjectiveParameters()
        {
            Name = "Test",
            Expectation = ObjectiveExpectation.NotMet,
            ResolutionBehavior = ObjectiveResolutionBehavior.SelfResolution
        };

        // Assert
        Assert.Throws<InvalidOperationException>(() => parameters.Validate());
    }

    #endregion

    #region GetEffectiveResolutionBehavior

    [Theory]
    [InlineData(ObjectiveExpectation.Met, ObjectiveResolutionBehavior.SelfResolution)]
    [InlineData(ObjectiveExpectation.NotMet, ObjectiveResolutionBehavior.ChallengeResolution)]
    public void GetEffectiveResolutionBehavior_VaryingExpectations_ReturnsExpectedBehavior(ObjectiveExpectation expectation, ObjectiveResolutionBehavior expectedBehavior)
    {
        // Arrange
        var parameters = new ObjectiveParameters
        {
            Name = "Test",
            Expectation = expectation
        };

        // Act/Assert
        Assert.Equal(expectedBehavior, parameters.GetEffectiveResolutionBehavior());
    }

    [Fact]
    public void GetEffectiveResolutionBehavior_ResolutionBehaviorSet_ReturnsExpected()
    {
        // Arrange
        var parameters = new ObjectiveParameters
        {
            Name = "Test",
            Expectation = ObjectiveExpectation.Met,
            ResolutionBehavior = ObjectiveResolutionBehavior.ChallengeResolution
        };

        // Act/Assert
        Assert.Equal(ObjectiveResolutionBehavior.ChallengeResolution, parameters.GetEffectiveResolutionBehavior());
    }

    #endregion
}
