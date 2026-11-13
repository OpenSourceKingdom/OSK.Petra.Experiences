using OSK.Petra.Experiences.Internal.Services;
using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Internal;

internal interface IExperienceContextProvider
{
    IInternalExperienceContext GetExperienceContext(IExperience experience, ExperienceState state);

    ObjectiveContext GetObjectiveContext(ExperienceState experienceState, ChallengeState challengeState);

    ExperienceTransitionContext GetTransitionContext(ExperienceState state);
}
