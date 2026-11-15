using Moq;
using OSK.Petra.Experiences.Events;
using OSK.Petra.Experiences.Internal;
using OSK.Petra.Experiences.Internal.Services;
using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.UnitTests.Internal.Services;

public class ExperienceContextTests
{
    #region Variables

    private readonly Mock<IExperience> _mockExperience;
    private readonly Mock<IExperienceContextProvider> _mockContextProvider;
    private readonly Mock<IExperienceEnumerator> _mockEnumerator;
    private readonly ExperienceState _state;

    private readonly ExperienceContext _context;

    #endregion

    #region Comstructors

    public ExperienceContextTests()
    {
        _mockExperience = new();
        _mockEnumerator = new();
        _mockExperience.Setup(m => m.GetEnumerator())
            .Returns(_mockEnumerator.Object);

        _mockContextProvider = new();
        _state = new();

        _context = new(_mockExperience.Object, _state, _mockContextProvider.Object);
    }

    #endregion

    #region Start_StatusAvailable_MovesToReady

    [Fact]
    public void Start_AvailableStatus_InitializesAndFiresEvent()
    {
        // Arrange
        var triggeredCount = 0;
        _context.OnEvent += experienceEvent =>
        {
            if (experienceEvent is ExperienceStatusChangeEvent)
            {
                triggeredCount++;
            }
        };
        _state.Status = ExperienceStatus.Available;

        // Act
        _context.Start();

        // Assert

        // No Challenges, so completes
        Assert.Equal(ExperienceStatus.Successful, _context.Status);
        Assert.Equal(1, triggeredCount);
    }

    [Fact]
    public void Start_ActiveStatus_NoActiveChallenge_InitializesAndFiresEvent()
    {
        // Arrange
        var triggeredCount = 0;
        _context.OnEvent += experienceEvent =>
        {
            if (experienceEvent is ExperienceStatusChangeEvent)
            {
                triggeredCount++;
            }
        };
        _state.Status = ExperienceStatus.Active;

        // Act
        _context.Start();

        // Assert

        // No Challenges, so completes
        Assert.Equal(ExperienceStatus.Successful, _context.Status);
        Assert.Equal(1, triggeredCount);
    }

    [Fact]
    public void Start_PausedStatus_SetsToActiveAndFiresEvent()
    {
        // Arrange
        var triggeredCount = 0;
        _context.OnEvent += experienceEvent =>
        {
            if (experienceEvent is ExperienceStatusChangeEvent)
            {
                triggeredCount++;
            }
        };
        _state.Status = ExperienceStatus.Paused;

        // Act
        _context.Start();

        // Assert
        Assert.Equal(ExperienceStatus.Active, _context.Status);
        Assert.Equal(1, triggeredCount);
    }

    [Theory]
    [InlineData(ExperienceStatus.Abandoned)]
    [InlineData(ExperienceStatus.Successful)]
    [InlineData(ExperienceStatus.Failed)]
    [InlineData(ExperienceStatus.Unavailable)]
    public void Start_NonAvailableStatus_HasNoEffect(ExperienceStatus status)
    {
        // Arrange
        var triggeredCount = 0;
        _context.OnEvent += experienceEvent =>
        {
            if (experienceEvent is ExperienceStatusChangeEvent)
            {
                triggeredCount++;
            }
        };

        _state.Status = status;

        // Act
        _context.Start();

        // Assert
        Assert.Equal(status, _context.Status);
        Assert.Equal(0, triggeredCount);
    }

    #endregion

    #region Stop

    [Fact]
    public void Stop_ActiveStatus_ExpeirencePauseStopBehavior_ChangesToPausedAndTriggersEvent()
    {
        // Arrange
        var triggeredCount = 0;
        _context.OnEvent += experienceEvent =>
        {
            if (experienceEvent is ExperienceStatusChangeEvent)
            {
                triggeredCount++;
            }
        };

        _mockExperience.Setup(m => m.StopBehavior)
            .Returns(ExperienceStopBehavior.Pause);

        _state.Status = ExperienceStatus.Active;

        // Act
        _context.Stop();

        // Assert
        Assert.Equal(ExperienceStatus.Paused, _context.Status);
    }

    [Theory]
    [InlineData(ExperienceStatus.Available)]
    [InlineData(ExperienceStatus.Successful)]
    [InlineData(ExperienceStatus.Failed)]
    [InlineData(ExperienceStatus.Abandoned)]
    [InlineData(ExperienceStatus.Paused)]
    [InlineData(ExperienceStatus.Unavailable)]
    public void Stop_NotActiveStatus_HasNoEffect(ExperienceStatus status)
    {
        // Arrange
        var triggeredCount = 0;
        _context.OnEvent += experienceEvent =>
        {
            if (experienceEvent is ExperienceStatusChangeEvent)
            {
                triggeredCount++;
            }
        };

        _mockExperience.Setup(m => m.StopBehavior)
            .Returns(ExperienceStopBehavior.Pause);

        _state.Status = status;

        // Act
        _context.Stop();

        // Assert
        Assert.Equal(status, _context.Status);
        Assert.Equal(0, triggeredCount);
    }

    [Fact]
    public void Stop_NotAllowStop_HasNoEffect()
    {        
        // Arrange
        var triggeredCount = 0;
        _context.OnEvent += experienceEvent =>
        {
            if (experienceEvent is ExperienceStatusChangeEvent)
            {
                triggeredCount++;
            }
        };

        _mockExperience.Setup(m => m.StopBehavior)
            .Returns(ExperienceStopBehavior.None);

        _state.Status = ExperienceStatus.Active;

        // Act
        _context.Stop();

        // Assert
        Assert.Equal(ExperienceStatus.Active, _context.Status);
        Assert.Equal(0, triggeredCount);
    }

    [Fact]
    public void Stop_Active_ResetStopBehavior_ClearsChallengesResetsToAvailable()
    {
        // Arrange
        var triggeredCount = 0;
        _context.OnEvent += experienceEvent =>
        {
            if (experienceEvent is ExperienceStatusChangeEvent)
            {
                triggeredCount++;
            }
        };

        _mockExperience.Setup(m => m.StopBehavior)
            .Returns(ExperienceStopBehavior.Reset);

        _state.Status = ExperienceStatus.Active;

        // Act
        _context.Stop();

        // Assert
        Assert.Equal(ExperienceStatus.Available, _context.Status);
        Assert.Equal(1, triggeredCount);
    }

    [Fact]
    public void Stop_Active_EndStopBehavior_Required_EnedBehaviorSetsToFailed()
    {
        // Arrange
        var triggeredCount = 0;
        _context.OnEvent += experienceEvent =>
        {
            if (experienceEvent is ExperienceStatusChangeEvent)
            {
                triggeredCount++;
            }
        };

        _mockExperience.Setup(m => m.StopBehavior)
            .Returns(ExperienceStopBehavior.End);
        _mockExperience.Setup(m => m.IsRequired)
            .Returns(true);

        _state.Status = ExperienceStatus.Active;

        // Act
        _context.Stop();

        // Assert
        Assert.Equal(ExperienceStatus.Failed, _context.Status);
        Assert.Equal(1, triggeredCount);
    }

    [Fact]
    public void Stop_Active_NotRquired_EnedBehaviorSetsToAbandoned()
    {
        // Arrange
        var triggeredCount = 0;
        _context.OnEvent += experienceEvent =>
        {
            if (experienceEvent is ExperienceStatusChangeEvent)
            {
                triggeredCount++;
            }
        };

        _mockExperience.Setup(m => m.StopBehavior)
            .Returns(ExperienceStopBehavior.End);

        _state.Status = ExperienceStatus.Active;

        // Act
        _context.Stop();

        // Assert
        Assert.Equal(ExperienceStatus.Abandoned, _context.Status);
        Assert.Equal(1, triggeredCount);
    }

    #endregion

    #region Pinned Setter

    [Fact]
    public void SetPinned_WasFalse_ThenTrue_FiresPinEvent()
    {
        // Arrange
        var triggeredCount = 0;
        _context.OnEvent += experienceEvent =>
        {
            if (experienceEvent is ExperiencePinEvent)
            {
                triggeredCount++;
            }
        };

        _state.IsPinned = false;


        // Act
        _context.Pinned = true;

        // Assert
        Assert.True(_context.Pinned);
        Assert.Equal(1, triggeredCount);
    }

    [Fact]
    public void SetPinned_WasTrue_ThenFalse_FiresPinEvent()
    {
        // Arrange
        var triggeredCount = 0;
        _context.OnEvent += experienceEvent =>
        {
            if (experienceEvent is ExperiencePinEvent)
            {
                triggeredCount++;
            }
        };

        _state.IsPinned = true;


        // Act
        _context.Pinned = false;

        // Assert
        Assert.False(_context.Pinned);
        Assert.Equal(1, triggeredCount);
    }


    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void SetPinned_SameValue_DoesNotFirePinEvent(bool isPinned)
    {
        // Arrange
        var triggeredCount = 0;
        _context.OnEvent += experienceEvent =>
        {
            if (experienceEvent is ExperiencePinEvent)
            {
                triggeredCount++;
            }
        };

        _state.IsPinned = isPinned;


        // Act
        _context.Pinned = isPinned;

        // Assert
        Assert.Equal(isPinned, _context.Pinned);
        Assert.Equal(0, triggeredCount);
    }

    #endregion

    #region Tick

    #endregion

    #region ProcessEvent


    [Fact]
    public void ProcessEvent_NullAppEvent_ThrowsArgumentNullException()
    {
        // Arrange
        _state.Status = ExperienceStatus.Active;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _context.ProcessEvent(null!));
    }

    #endregion

    #region TryGetOutput_CorrectlyRetrievesValues

    [Fact]
    public void TryGetOutput_ExistingKey_ReturnsTrueWithValue()
    {
        // Arrange
        _state.Outputs = new()
        {
            { "Hello", "What" }
        };

        // Act
        var result = _context.TryGetOutput<string>("Hello", out var output);

        // Assert
        Assert.True(result);
        Assert.Equal("What", output);
    }

    [Fact]
    public void TryGetOutput_NonExistentKey_ReturnsFalse()
    {
        // Arrange/Act
        var result = _context.TryGetOutput<string>("Hello", out var output);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryGetOutput_InvalidType_ReturnsFalse()
    {
        // Arrange
        _state.Outputs = new()
        {
            { "Hello", "What" }
        };

        // Act
        var result = _context.TryGetOutput<int>("Hello", out var output);

        // Assert
        Assert.False(result);
    }

    #endregion

    #region Dispose

    [Fact]
    public void Dispose_DisposesEnumerator()
    {
        // Arrange/Act
        _context.Dispose();

        // Assert
        _mockEnumerator.Verify(m => m.Dispose(), Times.Once);
    }

    #endregion
}
