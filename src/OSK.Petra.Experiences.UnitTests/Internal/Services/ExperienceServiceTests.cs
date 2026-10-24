using Moq;
using OSK.Petra.Experiences.Events;
using OSK.Petra.Experiences.Internal;
using OSK.Petra.Experiences.Internal.Services;
using OSK.Petra.Experiences.Models;
using OSK.Petra.Experiences.Options;
using OSK.Petra.Experiences.UnitTests._Helpers;
using System.Collections.Generic;
using System.Linq;

namespace OSK.Petra.Experiences.UnitTests.Internal.Services;

public class ExperienceServiceTests
{
    #region Variables

    private readonly Mock<IExperienceContextProvider> _mockContextProvider;
    private readonly ExperienceService _service;

    #endregion

    #region Constructors

    public ExperienceServiceTests()
    {
        _mockContextProvider = new();
        _mockContextProvider.Setup(m => m.GetExperienceContext(It.IsAny<IExperience>(), It.IsAny<ExperienceState>()))
            .Returns((IExperience experience, ExperienceState state) =>
            {
                var mockContext = new Mock<IInternalExperienceContext>();
                mockContext.Setup(m => m.Experience)
                    .Returns(experience);

                mockContext.Setup(m => m.Status)
                    .Returns(state.Status);

                return mockContext.Object;
            });

        _service = new ExperienceService(_mockContextProvider.Object);
    }

    #endregion

    #region Add

    [Fact]
    public void Add_NullExperience_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _service.Add((IExperience)null!));
    }

    [Fact]
    public void Add_ValidExperience_AddsToLookup_ReturnsContext()
    {
        // Arrange
        var triggeredCount = 0;
        _service.OnExperienceEvent += experienceEvent =>
        {
            if (experienceEvent is ExperienceAddedEvent)
            {
                triggeredCount++;
            }
        };

        var experience = new Mock<IExperience>();

        // Act
        var context = _service.Add(experience.Object);

        // Assert
        Assert.NotNull(context);
        Assert.Equal(experience.Object, context.Experience);
        Assert.Equal(1, triggeredCount);
    }

    [Fact]
    public void Add_DuplicateId_ReturnsExistingContext_DoesNotFireEvent()
    {
        // Arrange
        var triggeredCount = 0;
        _service.OnExperienceEvent += experienceEvent =>
        {
            if (experienceEvent is ExperienceAddedEvent)
            {
                triggeredCount++;
            }
        };

        var experience = new Mock<IExperience>();
        var context1 = _service.Add(experience.Object);

        // Act
        var context2 = _service.Add(experience.Object);

        // Assert
        Assert.Equal(context1, context2);
        Assert.Equal(1, triggeredCount);
    }

    #endregion

    #region GetContext

    [Fact]
    public void GetContext_ExistingId_ReturnsContext()
    {
        // Arrange
        var experience = new Mock<IExperience>();
        var context1 = _service.Add(experience.Object);

        // Act
        var context2 = _service.GetContext(experience.Object.Id);

        // Assert
        Assert.NotNull(context2);
        Assert.Equal(context1, context2);
    }

    [Fact]
    public void GetContext_NonExistentId_ReturnsNull()
    {
        // Arrange/Act
        var context = _service.GetContext(Guid.NewGuid());

        // Assert
        Assert.Null(context);
    }

    #endregion

    #region GetContexts

    [Fact]
    public void GetContexts_NoFilters_ReturnsAllContexts()
    {
        // Arrange
        var exp1 = new Mock<IExperience>();
        exp1.Setup(m => m.Id)
            .Returns(Guid.NewGuid());

        var exp2 = new Mock<IExperience>();
        exp2.Setup(m => m.Id)
            .Returns(Guid.NewGuid());

        _service.Add(exp1.Object);
        _service.Add(exp2.Object);

        // Act
        var contexts = _service.GetContexts();

        // Assert
        Assert.Equal(2, contexts.Count());
    }

    [Fact]
    public void GetExperiences_StatusFilter_MatchesActive_ReturnsMatchingOnly()
    {
        // Arrange
        var exp1 = new Mock<IExperience>();
        exp1.Setup(m => m.Id)
            .Returns(Guid.NewGuid());

        var exp2 = new Mock<IExperience>();
        exp2.Setup(m => m.Id)
            .Returns(Guid.NewGuid());

        _service.Add(exp1.Object, new()
        {
            State = new()
            {
                Status = ExperienceStatus.Active
            }
        });
        _service.Add(exp2.Object);

        var options = new ExperienceSearchOptions().WithStatus(ExperienceStatus.Active);

        // Act
        var contexts = _service.GetContexts(options);

        // Assert
        Assert.Single(contexts);
    }

    [Fact]
    public void GetExperiences_StatusFilter_NonMatching_ReturnsEmpty()
    {
        // Arrange
        var exp1 = new Mock<IExperience>();
        exp1.Setup(m => m.Id)
            .Returns(Guid.NewGuid());
        _service.Add(exp1.Object);

        var options = new ExperienceSearchOptions().WithStatus(ExperienceStatus.Complete);

        // Act
        var contexts = _service.GetContexts(options);

        // Assert
        Assert.Empty(contexts);
    }

    [Fact]
    public void GetExperiences_IsPinned_True_ReturnsOnlyPinned()
    {
        // Arrange
        var exp1 = new Mock<IExperience>();
        exp1.Setup(m => m.Id)
            .Returns(Guid.NewGuid());

        var exp2 = new Mock<IExperience>();
        exp2.Setup(m => m.Id)
            .Returns(Guid.NewGuid());

        var i = 0;
        _mockContextProvider.Setup(m => m.GetExperienceContext(It.IsAny<IExperience>(), It.IsAny<ExperienceState>()))
            .Returns((IExperience experience, ExperienceState state) =>
            {
                var mockContext = new Mock<IInternalExperienceContext>();
                mockContext.Setup(m => m.Experience)
                    .Returns(experience);

                mockContext.Setup(m => m.Status)
                    .Returns(state.Status);

                var pinned = i == 0;
                mockContext.Setup(m => m.Pinned)
                    .Returns(pinned);

                i++;

                return mockContext.Object;
            });

        _service.Add(exp1.Object);
        _service.Add(exp2.Object);

        var options = new ExperienceSearchOptions().WithPinned();

        // Act
        var contexts = _service.GetContexts(options);

        // Assert
        Assert.Single(contexts);
    }

    [Fact]
    public void GetExperiences_IsPinned_False_ReturnsOnlyUnpinned()
    {
        // Arrange
        var exp1 = new Mock<IExperience>();
        exp1.Setup(m => m.Id)
            .Returns(Guid.NewGuid());

        var exp2 = new Mock<IExperience>();
        exp2.Setup(m => m.Id)
            .Returns(Guid.NewGuid());

        var i = 0;
        _mockContextProvider.Setup(m => m.GetExperienceContext(It.IsAny<IExperience>(), It.IsAny<ExperienceState>()))
            .Returns((IExperience experience, ExperienceState state) =>
            {
                var mockContext = new Mock<IInternalExperienceContext>();
                mockContext.Setup(m => m.Experience)
                    .Returns(experience);

                mockContext.Setup(m => m.Status)
                    .Returns(state.Status);

                var pinned = i == 0;
                mockContext.Setup(m => m.Pinned)
                    .Returns(pinned);

                i++;

                return mockContext.Object;
            });

        _service.Add(exp1.Object);
        _service.Add(exp2.Object);

        var options = new ExperienceSearchOptions().WithouthPinned();

        // Act
        var contexts = _service.GetContexts(options);

        // Assert
        Assert.Single(contexts);
    }

    [Fact]
    public void GetExperiences_CombinedFilters_IntersectsBoth()
    {
        // Arrange
        var exp1 = new Mock<IExperience>();
        exp1.Setup(m => m.Id)
            .Returns(Guid.NewGuid());

        var exp2 = new Mock<IExperience>();
        exp2.Setup(m => m.Id)
            .Returns(Guid.NewGuid());

        var ctx1 = _service.Add(exp1.Object, new()
        {
            State = new()
            {
                Status = ExperienceStatus.Active
            }
        });
        var ctx2 = _service.Add(exp2.Object);
        ctx1.Pinned = true;

        var options = new ExperienceSearchOptions()
            .WithPinned()
            .WithStatus(ExperienceStatus.Active);

        // Act
        var contexts = _service.GetContexts(options);

        // Assert
        Assert.Single(contexts);
    }

    [Fact]
    public void GetExperiences_EmptyStatusFilterArray_ReturnsAll()
    {
        // Arrange
        var exp1 = new Mock<IExperience>();
        exp1.Setup(m => m.Id)
            .Returns(Guid.NewGuid());

        var exp2 = new Mock<IExperience>();
        exp2.Setup(m => m.Id)
            .Returns(Guid.NewGuid());

        _service.Add(exp1.Object, new()
        {
            State = new()
            {
                Status = ExperienceStatus.Active
            }
        });
        _service.Add(exp2.Object);

        var options = new ExperienceSearchOptions() { StatusFilter = [] };

        // Act
        var contexts = _service.GetContexts(options);

        // Assert
        Assert.Equal(2, contexts.Count());
    }

    #endregion

    #region Remove

    [Fact]
    public void Remove_ExistingId_DisposesAndRemoves_FiresRemovedEvent()
    {
        // Arrange
        var experience = new Mock<IExperience>();

        var triggeredEvent = 0;
        _service.OnExperienceEvent += experienceEvent =>
        {
            if (experienceEvent is ExperienceRemovedEvent)
            {
                triggeredEvent++;
            }
        };

        _service.Add(experience.Object);

        // Act
        var result = _service.Remove(experience.Object.Id);

        // Assert
        Assert.True(result);
        Assert.Equal(1, triggeredEvent);
        Assert.Null(_service.GetContext(experience.Object.Id));
    }

    [Fact]
    public void Remove_NonExistentId_ReturnsFalse_DoesNotFireEvent()
    {
        // Arrange
        var triggeredEvent = 0;
        _service.OnExperienceEvent += experienceEvent =>
        {
            if (experienceEvent is ExperienceRemovedEvent)
            {
                triggeredEvent++;
            }
        };

        // Act
        var result = _service.Remove(Guid.NewGuid());

        // Assert
        Assert.False(result);
        Assert.Equal(0, triggeredEvent);
    }

    #endregion

    #region Update

    [Fact]
    public void Update_ActiveExperience_TicksContext()
    {
        // Arrange
        var experience = new Mock<IExperience>();

        var mockContext = new Mock<IInternalExperienceContext>();
        mockContext.Setup(m => m.Experience)
            .Returns(experience.Object);

        _mockContextProvider.Setup(m => m.GetExperienceContext(It.IsAny<IExperience>(), It.IsAny<ExperienceState>()))
            .Returns((IExperience experience, ExperienceState state) =>
            {
                mockContext.Setup(m => m.Status)
                .Returns(ExperienceStatus.Active);
                return mockContext.Object;
            });

        _service.Add(experience.Object);

        // Act
        _service.Update(TimeSpan.FromSeconds(1));

        // Assert
        mockContext.Verify(m => m.Tick(It.IsAny<TimeSpan>()), Times.Once);
    }

    [Fact]
    public void Update_NoActiveExperience_DoesNothing()
    {
        // Arrange
        var experience = new Mock<IExperience>();

        var mockContext = new Mock<IInternalExperienceContext>();
        mockContext.Setup(m => m.Experience)
            .Returns(experience.Object);

        _mockContextProvider.Setup(m => m.GetExperienceContext(It.IsAny<IExperience>(), It.IsAny<ExperienceState>()))
            .Returns((IExperience experience, ExperienceState state) =>
            {
                mockContext.Setup(m => m.Status)
                .Returns(ExperienceStatus.Available);
                return mockContext.Object;
            });

        _service.Add(experience.Object);

        // Act
        _service.Update(TimeSpan.FromSeconds(1));

        // Assert
        mockContext.Verify(m => m.Tick(It.IsAny<TimeSpan>()), Times.Never);
    }

    #endregion

    #region ProcessEvent

    [Fact]
    public void ProcessEvent_NullEvent_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _service.ProcessEvent(null!));
    }

    [Fact]
    public void ProcessEvent_ActiveExperiences_ProcessesEventl()
    {
        // Arrange
        var experience = new Mock<IExperience>();

        var mockContext = new Mock<IInternalExperienceContext>();
        mockContext.Setup(m => m.Experience)
            .Returns(experience.Object);

        _mockContextProvider.Setup(m => m.GetExperienceContext(It.IsAny<IExperience>(), It.IsAny<ExperienceState>()))
            .Returns((IExperience experience, ExperienceState state) =>
            {
                mockContext.Setup(m => m.Status)
                .Returns(ExperienceStatus.Active);
                return mockContext.Object;
            });

        _service.Add(experience.Object);

        // Act
        _service.ProcessEvent(Mock.Of<IApplicationExperienceEvent>());

        // Assert
        mockContext.Verify(m => m.ProcessEvent(It.IsAny<IApplicationExperienceEvent>()), Times.Once);
    }

    [Fact]
    public void ProcessEvent_NoActiveExperiences_DoesNotProcessEventl()
    {
        // Arrange
        var experience = new Mock<IExperience>();

        var mockContext = new Mock<IInternalExperienceContext>();
        mockContext.Setup(m => m.Experience)
            .Returns(experience.Object);

        _mockContextProvider.Setup(m => m.GetExperienceContext(It.IsAny<IExperience>(), It.IsAny<ExperienceState>()))
            .Returns((IExperience experience, ExperienceState state) =>
            {
                mockContext.Setup(m => m.Status)
                .Returns(ExperienceStatus.Available);
                return mockContext.Object;
            });

        _service.Add(experience.Object);

        // Act
        _service.ProcessEvent(Mock.Of<IApplicationExperienceEvent>());

        // Assert
        mockContext.Verify(m => m.ProcessEvent(It.IsAny<IApplicationExperienceEvent>()), Times.Never);
    }

    #endregion
}
