using Moq;
using OSK.Petra.Experiences.Enumerators.Sequential;
using OSK.Petra.Experiences.Models;
using OSK.Petra.Experiences.UnitTests._Helpers;

namespace OSK.Petra.Experiences.UnitTests.Enumerators.Sequential;

public class SequentialChallengeEnumeratorTests
{
    #region Constructor Validations

    [Fact]
    public void Constructor_NullChallengeList_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new SequentialChallengeEnumerator(null!));
    }

    [Fact]
    public void Constructor_NegativeChallengeIndex_ThrowsArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new SequentialChallengeEnumerator([], challengeIndex: -1));
    }

    #endregion

    #region MoveNext

    [Fact]
    public void MoveNext_NoChallenges_ReturnsFalse()
    {
        // Arrange
        var enumerator = new SequentialChallengeEnumerator([]);

        // Act
        var result = enumerator.MoveNext(null!);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void MoveNext_AdvancesThroughAllChallenges()
    {
        // Arrange
        var tests = 3;
        var challenges = GetChallenges(tests);
        var enumerator = new SequentialChallengeEnumerator(challenges);

        // Act & Assert - first call advances from null to index 0

        Assert.Null(enumerator.Current);
        while (tests > 0)
        {
            Assert.True(enumerator.MoveNext(null!));
            Assert.NotNull(enumerator.Current);

            tests--;
        }

        Assert.False(enumerator.MoveNext(null!));
        Assert.Null(enumerator.Current);
    }

    [Fact]
    public void MoveNext_ValidStartIndex_ReturnsFirstChallenge()
    {
        // Arrange
        var challenges = GetChallenges(3);
        var enumerator = new SequentialChallengeEnumerator(challenges, challengeIndex: 1);

        // Act & Assert
        Assert.True(enumerator.MoveNext(null!));
        Assert.Equal(challenges[1], enumerator.Current);
    }

    #endregion

    #region Reset

    [Fact]
    public void Reset_FromMidState_IndexClearsForRestart()
    {
        // Arrange
        var challenges = GetChallenges(3);
        var enumerator = new SequentialChallengeEnumerator(challenges);
        Assert.True(enumerator.MoveNext(null!));
        Assert.NotNull(enumerator.Current);

        // Act
        enumerator.Reset();

        // Assert
        Assert.Null(enumerator.Current);

        // Should be started at the start
        Assert.True(enumerator.MoveNext(null!));
        Assert.Equal(challenges[0], enumerator.Current);
    }

    [Fact]
    public void Reset_EndOfChallenges_RestartsFromBeginning()
    {
        // Arrange
        var challenges = GetChallenges(3);
        var enumerator = new SequentialChallengeEnumerator(challenges);

        // Exhaust all
        Assert.True(enumerator.MoveNext(null!));
        Assert.True(enumerator.MoveNext(null!));
        Assert.True(enumerator.MoveNext(null!));

        // Act
        enumerator.Reset();

        // Assert
        Assert.Null(enumerator.Current);
        Assert.True(enumerator.MoveNext(null!));
        Assert.Equal(challenges[0], enumerator.Current);
    }

    #endregion

    #region Helpers

    private List<IExperienceChallenge> GetChallenges(int count)
    {
        var list = new List<IExperienceChallenge>();
        for (int i = 0; i < count; i++)
        {
            var mockChallenge = new Mock<IExperienceChallenge>();
            mockChallenge.SetupGet(m => m.Id)
                .Returns(i);
            list.Add(mockChallenge.Object);
        }
        return list;
    }

    #endregion
}
