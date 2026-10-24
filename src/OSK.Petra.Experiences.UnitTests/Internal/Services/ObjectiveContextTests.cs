using Moq;
using OSK.Petra.Experiences.Internal.Services;
using OSK.Petra.Experiences.Models;
using OSK.Petra.Experiences.UnitTests._Helpers;

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
        Assert.Throws<ArgumentNullException>(() => new ObjectiveContext(null!, challengeState, [], _mockServices.Object));
    }

    [Fact]
    public void Constructor_NullChallengeState_ThrowsArgumentNullException()
    {
        // Arrange
        var experienceState = new ExperienceState();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ObjectiveContext(experienceState, null!, [], _mockServices.Object));
    }

    [Fact]
    public void Constructor_NullObjectives_ThrowsArgumentNullException()
    {
        // Arrange
        var experienceState = new ExperienceState();
        var challengeState = new ChallengeState();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ObjectiveContext(experienceState, challengeState, null!, _mockServices.Object));
    }

    [Fact]
    public void Constructor_NullServiceProvider_ThrowsArgumentNullException()
    {
        // Arrange
        var experienceState = new ExperienceState();
        var challengeState = new ChallengeState();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ObjectiveContext(experienceState, challengeState, [], null!));
    }

    [Fact]
    public void Constructor_Valid_InitializesExpected()
    {
        // Arrange
        var experienceState = new ExperienceState();

        var objectiveState = new ObjectiveState()
        {
            ObjectiveId = 1
        };
        var challengeState = new ChallengeState()
        {
            ChallengeId = 0,
            ObjectiveStates = new Dictionary<int, ObjectiveState>()
            {
                { 1, objectiveState }
            }
        };
        var objectives = new List<IChallengeObjective>
        {
            Mock.Of<IChallengeObjective>(),
            Mock.Of<IChallengeObjective>(),
            Mock.Of<IChallengeObjective>()
        };

        // Act
        var context = new ObjectiveContext(experienceState, challengeState, objectives, _mockServices.Object);

        // Assert
        Assert.Equal(context.Services, _mockServices.Object);

        Assert.Equal(3, challengeState.ObjectiveStates.Count);
        foreach (var state in challengeState.ObjectiveStates)
        {
            Assert.Equal(ObjectiveStatus.Active, state.Value.Status);

            if (state.Key is 1)
            {
                Assert.Equal(state.Value, objectiveState);
            }
        }
    }
    
    #endregion

    #region Current

    [Fact]
    public void Current_Initial_IsNull()
    {
        // Arrange
        var experienceState = new ExperienceState();
        var challengeState = new ChallengeState();
        var objectives = new List<IChallengeObjective> { Mock.Of<IChallengeObjective>() };
        var context = new ObjectiveContext(experienceState, challengeState, objectives, _mockServices.Object);

        // Act/Assert
        Assert.Null(context.Current);
    }

    [Fact]
    public void Current_Moves_ReturnsExpected()
    {
        // Arrange
        var experienceState = new ExperienceState();
        var challengeState = new ChallengeState();
        var objective = Mock.Of<IChallengeObjective>();
        var objectives = new List<IChallengeObjective> { objective };
        var context = new ObjectiveContext(experienceState, challengeState, objectives, _mockServices.Object);

        // Act
        var moved = context.MoveNext();

        // Assert
        Assert.True(moved);
        Assert.NotNull(context.Current);
        Assert.Equal(objective, context.Current.Value.Objective);
    }

    #endregion

    #region Resolve

    [Fact]
    public void Resolve_True_SetsResolvedStatusToComplete()
    {
        // Arrange
        var experienceState = new ExperienceState();
        var challengeState = new ChallengeState();

        var objectives = new List<IChallengeObjective> { Mock.Of<IChallengeObjective>() };
        var context = new ObjectiveContext(experienceState, challengeState, objectives, _mockServices.Object);

        context.MoveNext();

        // Act
        context.Resolve(true);

        // Assert
        Assert.Equal(ObjectiveStatus.Complete, context.ResolvedStatus);
        Assert.False(context.ChallengeFailed);
    }

    [Fact]
    public void Resolve_False_ObjectiveRequired_SetsChallengeFailed()
    {
        // Arrange
        var experienceState = new ExperienceState();
        var challengeState = new ChallengeState();
        var mockObjective = new Mock<IChallengeObjective>();
        mockObjective.SetupGet(m => m.IsRequired)
            .Returns(true);

        var objectives = new List<IChallengeObjective> { mockObjective.Object };
        var context = new ObjectiveContext(experienceState, challengeState, objectives, _mockServices.Object);

        context.MoveNext();

        // Act
        context.Resolve(false);

        // Assert
        Assert.True(context.ChallengeFailed);
        Assert.Equal(ObjectiveStatus.Failed, context.ResolvedStatus);
    }

    [Fact]
    public void Resolve_False_ObjectiveNotRequired_DoesNotFailChallenge()
    {
        // Arrange
        var experienceState = new ExperienceState();
        var challengeState = new ChallengeState();
        var objectives = new List<IChallengeObjective> { Mock.Of<IChallengeObjective>() };
        var context = new ObjectiveContext(experienceState, challengeState, objectives, _mockServices.Object);

        context.MoveNext();

        // Act
        context.Resolve(false);

        // Assert
        Assert.False(context.ChallengeFailed);
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
        var objectives = new List<IChallengeObjective> { Mock.Of<IChallengeObjective>() };
        var context = new ObjectiveContext(experienceState, challengeState, objectives, _mockServices.Object);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => context.SetOutput(key!, "Test"));
    }

    #endregion

    #region MoveNext

    [Fact]
    public void MoveNext_AfterLastItem_ReturnsFalse()
    {
        // Arrange
        var experienceState = new ExperienceState();
        var challengeState = new ChallengeState();
        var objectives = new List<IChallengeObjective>
        {
            Mock.Of<IChallengeObjective>(),
            Mock.Of<IChallengeObjective>()
        };
        var context = new ObjectiveContext(experienceState, challengeState, objectives, _mockServices.Object);

        // Move through all items first
        Assert.True(context.MoveNext());
        Assert.True(context.MoveNext());

        // Act
        var result = context.MoveNext();

        // Assert
        Assert.False(result);
        Assert.Null(context.Current);
    }

    #endregion

    #region Reset

    [Fact]
    public void Reset_MovesToEnd_ReturnsToStart()
    {
        // Arrange
        var experienceState = new ExperienceState();
        var challengeState = new ChallengeState();
        var objectives = new List<IChallengeObjective>
        {
            Mock.Of<IChallengeObjective>(),
            Mock.Of<IChallengeObjective>()
        };
        var context = new ObjectiveContext(experienceState, challengeState, objectives, _mockServices.Object);

        // Move through all items first
        Assert.True(context.MoveNext());
        Assert.True(context.MoveNext());
        Assert.False(context.MoveNext());

        // Act
        context.Reset();

        // Assert
        Assert.True(context.MoveNext());
    }

    #endregion
}
