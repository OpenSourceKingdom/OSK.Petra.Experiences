using Moq;
using OSK.Petra.Experiences.Internal.Services;
using OSK.Petra.Experiences.Models;
using OSK.Petra.Experiences.Ports;

namespace OSK.Petra.Experiences.UnitTests.Internal.Services;

public class ExperienceTransitionContextTests
{
    #region Variables

    private readonly Mock<IExperienceService> _mockExperienceService;
    private readonly Mock<IServiceProvider> _mockServices;
    private readonly ExperienceState _state;

    private readonly ExperienceTransitionContext _context;

    #endregion

    #region Constructors

    public ExperienceTransitionContextTests()
    {
        _state = new ExperienceState();
        _mockServices = new();
        _mockExperienceService = new();

        _context = new ExperienceTransitionContext(_state, _mockExperienceService.Object, _mockServices.Object);
    }

    #endregion

    #region TryGetOutput

    [Fact]
    public void TryGetOutput_ExistingKey_ReturnsTrue()
    {
        // Arrange
        _state.Outputs["key1"] = "value1";

        // Act
        var result = _context.TryGetOutput<string>("key1", out var output);

        // Assert
        Assert.True(result);
        Assert.Equal(_state.Outputs["key1"] as string, output);
    }

    [Fact]
    public void TryGetOutput_NonExistentKey_ReturnsFalse()
    {
        // Arrange/Act
        var result = _context.TryGetOutput<string>("nonexistent", out var output);

        // Assert
        Assert.False(result);
        Assert.Null(output);
    }

    [Fact]
    public void TryGetOutput_WrongType_ReturnsFalse()
    {
        // Arrange
        _state.Outputs["stringKey"] = "value";

        // Act
        var result = _context.TryGetOutput<int>("stringKey", out var output);

        // Assert
        Assert.False(result);    
    }

    [Fact]
    public void TryGetOutput_WithExperienceId_FindsInRelatedExperience_ReturnsTrue()
    {
        // Arrange
        var mockContext = new Mock<IExperienceContext>();
        _mockExperienceService.Setup(m => m.GetContext(It.IsAny<Guid>()))
            .Returns(mockContext.Object);

        var expected = "hello";

        mockContext
            .Setup(m => m.TryGetOutput("test", out expected))
            .Returns(true);

        // Act
        var result = _context.TryGetOutput<string>(Guid.Empty, "test", out var output);

        // Assert
        Assert.True(result);
        Assert.Equal(expected, output);
    }

    [Fact]
    public void TryGetOutput_WithNonExistentExperienceId_ReturnsFalse()
    {
        // Arrange
        var mockContext = new Mock<IExperienceContext>();
        _mockExperienceService.Setup(m => m.GetContext(It.IsAny<Guid>()))
            .Returns(mockContext.Object);

        string? expected = null;

        mockContext
            .Setup(m => m.TryGetOutput("test", out expected))
            .Returns(false);

        // Act
        var result = _context.TryGetOutput<string>("test", out var output);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryGetOutput_WithExperienceId_NonMatchingType_ReturnsFalse()
    {
        // Arrange
        var mockContext = new Mock<IExperienceContext>();
        _mockExperienceService.Setup(m => m.GetContext(It.IsAny<Guid>()))
            .Returns(mockContext.Object);

        var expected = "hello";

        mockContext
            .Setup(m => m.TryGetOutput("test", out expected))
            .Returns(true);

        // Act
        var result = _context.TryGetOutput<int>("test", out var output);

        // Assert
        Assert.False(result);
    }

    #endregion
}
