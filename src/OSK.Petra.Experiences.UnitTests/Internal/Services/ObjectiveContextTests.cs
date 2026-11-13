using Moq;
using OSK.Petra.Experiences.Internal.Services;
using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.UnitTests.Internal.Services;

public class ObjectiveContextTests
{
    #region Variables

    private readonly Mock<IServiceProvider> _mockServices;

    #endregion

    #region Constructors

    public ObjectiveContextTests()
    {
        _mockServices = new();
    }

    #endregion

    #region Constructors (Validation)

    [Fact]
    public void Constructor_NullExperienceState_ThrowsArgumentNullException()
    {
        // Arrange
        var challengeState = new ChallengeState();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ObjectiveContext(null!, challengeState, _mockServices.Object));
    }

    [Fact]
    public void Constructor_NullChallengeState_ThrowsArgumentNullException()
    {
        // Arrange
        var experienceState = new ExperienceState();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ObjectiveContext(experienceState, null!, _mockServices.Object));
    }

    [Fact]
    public void Constructor_NullServiceProvider_ThrowsArgumentNullException()
    {
        // Arrange
        var experienceState = new ExperienceState();
        var challengeState = new ChallengeState();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ObjectiveContext(experienceState, challengeState, null!));
    }

    [Fact]
    public void Constructor_Valid_InitializesExpected()
    {
        // Arrange
        var experienceState = new ExperienceState();

        var objectiveState = new ObjectiveState()
        {
            ObjectiveId = "hi"
        };
        var challengeState = new ChallengeState()
        {
            ChallengeId = "0",
            ObjectiveStates = new Dictionary<string, ObjectiveState>()
            {
                { "1", objectiveState }
            }
        };

        // Act
        var context = new ObjectiveContext(experienceState, challengeState, _mockServices.Object);

        // Assert
        Assert.Equal(context.Services, _mockServices.Object);

        Assert.Single(challengeState.ObjectiveStates);
        foreach (var state in challengeState.ObjectiveStates)
        {
            Assert.Equal(ObjectiveStatus.Active, state.Value.Status);

            if (state.Key is "1")
            {
                Assert.Equal(state.Value, objectiveState);
            }
        }
    }
    
    #endregion

    #region Resolve

    [Fact]
    public void Resolve_True_SetsResolvedStatusToComplete()
    {
        // Arrange
        var experienceState = new ExperienceState();
        var challengeState = new ChallengeState();

        var context = new ObjectiveContext(experienceState, challengeState, _mockServices.Object);

        // Act
        context.Resolve(true);

        // Assert
        Assert.Equal(ObjectiveStatus.Complete, context.ResolvedStatus);
    }

    [Fact]
    public void Resolve_False_SetsResolvedStatusToFailed()
    {
        // Arrange
        var experienceState = new ExperienceState();
        var challengeState = new ChallengeState();
        var context = new ObjectiveContext(experienceState, challengeState, _mockServices.Object);

        // Act
        context.Resolve(false);

        // Assert
        Assert.Equal(ObjectiveStatus.Failed, context.ResolvedStatus);
    }

    #endregion

    #region SetOutput

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void SetOutput_InvalidKey_ThrowsArgumentException(string? key)
    {
        // Arrange
        var experienceState = new ExperienceState();
        var challengeState = new ChallengeState();
        var context = new ObjectiveContext(experienceState, challengeState, _mockServices.Object);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => context.SetOutput(key!, "Test"));
    }

    #endregion
}
