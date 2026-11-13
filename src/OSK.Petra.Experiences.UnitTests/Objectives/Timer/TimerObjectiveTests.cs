using Moq;
using OSK.Petra.Experiences.Models;
using OSK.Petra.Experiences.Objectives.Timer;

namespace OSK.Petra.Experiences.UnitTests.Objectives.Timer;

public class TimerObjectiveTests
{
    #region Variables

    private readonly Mock<IServiceProvider> _mockServices;

    #endregion

    #region Constructors

    public TimerObjectiveTests()
    {
        _mockServices = new();
    }

    #endregion

    #region ProcessTick

    [Fact]
    public void ProcessTick_DoesNotComplete_DoesNotResolve()
    {
        // Arrange
        var mockContext = new Mock<IObjectiveContext>();
        var timer = new TimerObjective(new() { Id = "Abc", Timer = TimeSpan.FromMinutes(1)});

        // Act
        timer.ProcessTick(mockContext.Object, TimeSpan.FromSeconds(30));

        // Assert
        mockContext.Verify(m => m.Resolve(It.IsAny<bool>()), Times.Never());

    }


    [Fact]
    public void ProcessTick_Completes_Resolves()
    {
        // Arrange
        var mockContext = new Mock<IObjectiveContext>();
        TimerState s;
        mockContext.Setup(m => m.SetState(It.IsAny<TimerState>()))
            .Callback((TimerState state) =>
            {
                s = state;
            });
        mockContext.Setup(m => m.TryGetState(out s))
            .Returns(true);

        var timer = new TimerObjective(new() { Id = "Abc", Timer = TimeSpan.FromMinutes(1) });

        // Act
        timer.ProcessTick(mockContext.Object, TimeSpan.FromSeconds(30));

        // Assert
        mockContext.Verify(m => m.Resolve(It.IsAny<bool>()), Times.Once());

    }

    #endregion

    #region Resolve

    [Fact]
    public void Resolve_TimerExpired_ExpectationMet_ResolvesSatisfied()
    {
        // Arrange
        var mockContext = new Mock<IObjectiveContext>();

        TimerState s;
        mockContext.Setup(m => m.SetState(It.IsAny<TimerState>()))
            .Callback((TimerState state) =>
            {
                s = state;
            });
        mockContext.Setup(m => m.TryGetState(out s))
            .Returns(true);

        var timer = new TimerObjective(new() { Id = "Abc", Timer = TimeSpan.FromMinutes(1) });

        // Act
        timer.ProcessTick(mockContext.Object, TimeSpan.FromMinutes(1));

        // Asert
        mockContext.Verify(m => m.Resolve(It.Is<bool>(b => b)), Times.Once());
        mockContext.Verify(m => m.Resolve(It.Is<bool>(b => !b)), Times.Never);
    }

    [Fact]
    public void Resolve_TimerNotExpired_ExpectationMet_ResolvesNotSatisfied()
    {
        // Arrange
        var mockContext = new Mock<IObjectiveContext>();
        var timer = new TimerObjective(new() { Id = "Abc", Timer = TimeSpan.FromMinutes(1) });

        // Act
        timer.ProcessTick(mockContext.Object, TimeSpan.FromMinutes(1));

        // Asert
        mockContext.Verify(m => m.Resolve(It.Is<bool>(b => b)), Times.Never());
        mockContext.Verify(m => m.Resolve(It.Is<bool>(b => !b)), Times.Once);
    }

    #endregion
}
