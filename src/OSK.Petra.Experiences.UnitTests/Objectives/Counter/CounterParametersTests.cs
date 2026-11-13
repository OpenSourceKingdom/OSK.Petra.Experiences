using Moq;
using OSK.Petra.Experiences.Objectives.Counter;

namespace OSK.Petra.Experiences.UnitTests.Objectives.Counter;

public class CounterParametersTests
{
    #region Static Creators

    [Fact]
    public void Goal_MetExpectation_SetsDefaultComparison()
    {
        // Arrange
        var test = false;

        var parameters = CounterParameters.Goal("Reach 10", thresholdValue: 10, c =>
        {
            c.WithFilter(_ => test = true)
            .WithTargets(["hello"])
            .WithTags(["hello"])
            .NormalizeValue();
        });

        // Assert
        Assert.Equal(CountComparison.GreaterThanOrEqual, parameters.Comparison);

        Assert.NotNull(parameters.TargetIds);
        Assert.Contains("hello", parameters.TargetIds);

        Assert.NotNull(parameters.Tags);
        Assert.Contains("hello", parameters.Tags);

        Assert.True(parameters.Normalize);

        Assert.NotNull(parameters.FilterPredicate);
        Assert.True(parameters.FilterPredicate(Mock.Of<ICounterEvent>()));
        Assert.True(test);
    }

    [Fact]
    public void Limit_NotMetExpectation_SetsDefaultComparison()
    {
        // Arrange
        var test = false;
        var parameters = CounterParameters.Limit("Keep below 5", thresholdValue: 5, c =>
        {
            c
            .WithFilter(_ => test = true)
            .WithTargets(["hello"])
            .WithTags(["hello"])
            .NormalizeValue();
        });

        // Assert
        Assert.Equal(CountComparison.GreaterThan, parameters.Comparison);

        Assert.NotNull(parameters.TargetIds);
        Assert.Contains("hello", parameters.TargetIds);

        Assert.NotNull(parameters.Tags);
        Assert.Contains("hello", parameters.Tags);

        Assert.True(parameters.Normalize);

        Assert.NotNull(parameters.FilterPredicate);
        Assert.True(parameters.FilterPredicate(Mock.Of<ICounterEvent>()));
        Assert.True(test);
    }

    #endregion

    #region Freeze

    [Fact]
    public void Freeze_Modify_ThrowsInvalidOperationException()
    {
        // Arrange
        var parameters = CounterParameters.Goal("Reach 10", thresholdValue: 10);

        // Act
        parameters.Freeze();

        // Assert
        Assert.Throws<InvalidOperationException>(() => parameters.WithTags(["tag1"]));
    }

    [Fact]
    public void Freeze_ModifyWithTargets_ThrowsInvalidOperationException()
    {
        // Arrange
        var parameters = CounterParameters.Goal("Reach 10", thresholdValue: 10);


        // Act
        parameters.Freeze();

        // Assert
        Assert.Throws<InvalidOperationException>(() => parameters.WithTargets(["tag1"]));
    }

    [Fact]
    public void Freeze_ModifyWithComparison_ThrowsInvalidOperationException()
    {
        // Arrange
        var parameters = CounterParameters.Goal("Reach 10", thresholdValue: 10);

        // Act
        parameters.Freeze();

        // Assert
        Assert.Throws<InvalidOperationException>(() => parameters.WithComparison(CountComparison.Equal));
    }

    [Fact]
    public void Freeze_ModifyWithFilter_ThrowsInvalidOperationException()
    {
        // Arrange
        var parameters = CounterParameters.Goal("Reach 10", thresholdValue: 10);


        // Act
        parameters.Freeze();

        // Assert
        Assert.Throws<InvalidOperationException>(() => parameters.WithFilter(_ => true));
    }

    [Fact]
    public void Freeze_ModifyNormalize_ThrowsInvalidOperationException()
    {
        // Arrange
        var parameters = CounterParameters.Goal("Reach 10", thresholdValue: 10);

        // Act
        parameters.Freeze();

        // Assert
        Assert.Throws<InvalidOperationException>(() => parameters.NormalizeValue());
    }

    #endregion

    #region Matches

    [Fact]
    public void Matches_NullCounterEvent_ThrowsArgumentNullException()
    {
        // Arrange
        var parameters = CounterParameters.Goal("Reach 10", thresholdValue: 10);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => parameters.Matches(null!));
    }

    [Fact]
    public void Matches_NoFilters_AcceptsAnyEvent()
    {
        // Arrange
        var parameters = CounterParameters.Goal("Reach 10", thresholdValue: 10);

        var mockEvent = new Mock<ICounterEvent>();

        // Act
        var result = parameters.Matches(mockEvent.Object);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Matches_WithTagsMatching_ReturnsTrue()
    {
        // Arrange
        var parameters = CounterParameters.Goal("Reach 10", thresholdValue: 10, c =>
        {
            c.WithTags(["wolf", "boss"]);
        });

        var mockEvent = new Mock<ICounterEvent>();
        mockEvent.Setup(m => m.Tags)
            .Returns(["wolf"]);

        // Act
        var result = parameters.Matches(mockEvent.Object);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Matches_WithTagsNoMatch_ReturnsFalse()
    {
        // Arrange
        var parameters = CounterParameters.Goal("Reach 10", thresholdValue: 10, c =>
        {
            c.WithTags(["wolf"]);
        });

        var mockEvent = new Mock<ICounterEvent>();
        mockEvent.Setup(m => m.Tags)
            .Returns(["cat"]);

        // Act
        var result = parameters.Matches(mockEvent.Object);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Matches_WithTargetsMatching_ReturnsTrue()
    {
        // Arrange
        var parameters = CounterParameters.Goal("Reach 10", thresholdValue: 10, c =>
        {
            c.WithTargets(["enemy-42"]);
        });

        var mockEvent = new Mock<ICounterEvent>();
        mockEvent.Setup(m => m.TargetId)
            .Returns("enemy-42");

        // Act
        var result = parameters.Matches(mockEvent.Object);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Matches_WithNoTagsButFilter_PassthroughTagCheck()
    {
        // Arrange
        var parameters = CounterParameters.Goal("Reach 10", thresholdValue: 10, c =>
        {
            c.WithFilter(_ => true);
        });

        var mockEvent = new Mock<ICounterEvent>();

        // Act
        var result = parameters.Matches(mockEvent.Object);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Matches_WithTagCountZeroButHasTagFilter_FailsMatch()
    {
        // Arrange
        var parameters = CounterParameters.Goal("Reach 10", thresholdValue: 10, c =>
        {
            c.WithTargets(["required"]);
        });

        var mockEvent = new Mock<ICounterEvent>();
        mockEvent.Setup(m => m.Tags)
            .Returns([]);

        // Act
        var result = parameters.Matches(mockEvent.Object);

        // Assert
        Assert.False(result);
    }

    #endregion
}
