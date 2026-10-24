using OSK.Petra.Experiences.Internal.Services;
using OSK.Petra.Experiences.Models;
using System.Collections.Generic;

namespace OSK.Petra.Experiences.Internal;

internal interface IExperienceContextProvider
{
    IInternalExperienceContext GetExperienceContext(IExperience experience, ExperienceState state);

    ObjectiveContext GetObjectiveContext(ExperienceState experienceState, ChallengeState challengeState, IReadOnlyList<IChallengeObjective> objectives);

    ExperienceTransitionContext GetTransitionContext(ExperienceState state);
}
