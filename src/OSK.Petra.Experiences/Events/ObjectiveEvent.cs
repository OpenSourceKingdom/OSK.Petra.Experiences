using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

public abstract class ObjectiveEvent(IExperience experience, IExperienceChallenge challenge, IChallengeObjective objective): ChallengeEvent(experience, challenge), IChallengeObjectiveEvent
{
    public IChallengeObjective Objective { get; } = objective;
}
