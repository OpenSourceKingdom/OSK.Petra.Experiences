using Microsoft.Extensions.DependencyInjection;
using OSK.Petra.Experiences.Models;
using System;
using System.Collections.Generic;

namespace OSK.Petra.Experiences.Internal.Services;

internal class ExperienceContextProvider(IServiceProvider services) : IExperienceContextProvider
{
    #region IExperienceContextProvider

    public IInternalExperienceContext GetExperienceContext(IExperience experience, ExperienceState state)
        => ActivatorUtilities.CreateInstance<ExperienceContext>(services, [experience, state ?? new()]);

    public ObjectiveContext GetObjectiveContext(ExperienceState experienceState, ChallengeState challengeState, IReadOnlyList<IChallengeObjective> objectives)
        => ActivatorUtilities.CreateInstance<ObjectiveContext>(services, [experienceState, challengeState, objectives]);

    public ExperienceTransitionContext GetTransitionContext(ExperienceState state)
        => ActivatorUtilities.CreateInstance<ExperienceTransitionContext>(services, [state ?? new()]);

    #endregion
}
