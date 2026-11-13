using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

public abstract class ObjectiveEvent(IExperienceContext context, IExperienceChallenge challenge, IChallengeObjective objective): ChallengeEvent(context, challenge), IChallengeObjectiveEvent
{
    public IChallengeObjective Objective { get; } = objective;
}
