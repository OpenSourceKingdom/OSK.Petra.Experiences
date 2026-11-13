using Microsoft.Extensions.DependencyInjection;
using Moq;
using OSK.Petra.Experiences.Internal;
using OSK.Petra.Experiences.Internal.Services;
using OSK.Petra.Experiences.Models;
using OSK.Petra.Experiences.Ports;
using OSK.Petra.Experiences.UnitTests._Helpers;

namespace OSK.Petra.Experiences.UnitTests.Internal.Services;

public class ExperienceContextProviderTests
{
    #region Variables

    private readonly IServiceProvider _services;

    private readonly ExperienceContextProvider _provider;

    #endregion

    #region Constructors

    public ExperienceContextProviderTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<IExperienceContextProvider>(_ => _provider!);
        serviceCollection.AddTransient<IExperienceService>(_ => Mock.Of<IExperienceService>());

        _services = serviceCollection.BuildServiceProvider();

        _provider = new ExperienceContextProvider(_services);
    }

    #endregion

    #region GetExperienceContext

    [Fact]
    public void GetExperienceContext_ReturnsContext()
    {
        // Arrange
        var experience = new TestExperience();
        var state = new ExperienceState();

        // Act
        var context = _provider.GetExperienceContext(experience, state);

        // Assert
        Assert.NotNull(context);
        Assert.Equal(experience, context.Experience);    
    }

    #endregion

    #region GetObjectiveContext

    [Fact]
    public void GetObjectiveContext_ReturnsContext()
    {
        // Arrange
        var experienceState = new ExperienceState();
        var challengeState = new ChallengeState();

        // Act
        var context = _provider.GetObjectiveContext(experienceState, challengeState);

        // Assert
        Assert.NotNull(context);
    }

    #endregion

    #region GetTransitionContext

    [Fact]
    public void GetTransitionContext_ReturnsContext()
    {
        // Arrange
        var state = new ExperienceState();

        // Act
        var context = _provider.GetTransitionContext(state);

        // Assert
        Assert.NotNull(context);
    }

    #endregion
}
