using OSK.Petra.Experiences.Models;
using OSK.Petra.Experiences.Objectives.Timer;

namespace OSK.Petra.Experiences.UnitTests.Objectives.Timer;

public class TimerParametersTests
{
    #region Static Creators

    [Fact]
    public void Duration_SetExpectedFields()
    {
        // Arrange & Act
        var parameters = TimerParameters.Duration("Countdown", "A timer", TimeSpan.FromSeconds(60));

        // Assert
        Assert.NotNull(parameters);
        Assert.Equal("Countdown", parameters.Name);
        Assert.Equal("A timer", parameters.Description);
        Assert.Equal(TimeSpan.FromSeconds(60), parameters.Timer);
        Assert.Equal(ObjectiveExpectation.Met, parameters.Expectation);
        Assert.Equal(ObjectiveResolutionBehavior.SelfResolution, parameters.GetEffectiveResolutionBehavior());
    }

    [Fact]
    public void Alarm_SetExpectedFields()
    {
        // Arrange & Act
        var parameters = TimerParameters.Alarm("Beep", TimeSpan.FromSeconds(30));

        // Assert
        Assert.NotNull(parameters);
        Assert.Equal("Beep", parameters.Name);
        Assert.Empty(parameters.Description);
        Assert.Equal(TimeSpan.FromSeconds(30), parameters.Timer);
        Assert.Equal(ObjectiveExpectation.NotMet, parameters.Expectation);
        Assert.Equal(ObjectiveResolutionBehavior.ChallengeResolution, parameters.GetEffectiveResolutionBehavior());
    }

    #endregion

    #region Validation

    [Fact]
    public void Validate_ZeroTimeSpan_ThrowsInvalidOperationException()
    {
        // Arrange/Act/Assert
        Assert.Throws<InvalidOperationException>(() => TimerParameters.Create("Test", string.Empty, TimeSpan.Zero, true, ObjectiveExpectation.Met));
    }

    [Fact]
    public void Validate_NegativeTimeSpan_ThrowsInvalidOperationException()
    {
        // Arrange/Act/Assert
        Assert.Throws<InvalidOperationException>(() => TimerParameters.Create("Test", string.Empty, TimeSpan.FromMinutes(-1), true, ObjectiveExpectation.Met));
    }

    #endregion
}
