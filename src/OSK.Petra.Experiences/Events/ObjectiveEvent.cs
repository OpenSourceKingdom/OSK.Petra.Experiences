using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

/// <summary>
/// An event for objectives that is triggered by the service
/// </summary>
/// <param name="context">The context for the experience</param>
/// <param name="challenge">The challenge</param>
/// <param name="objective">The objective</param>
public abstract class ObjectiveEvent(IExperienceContext context, IExperienceChallenge challenge, IChallengeObjective objective): ChallengeEvent(context, challenge), IChallengeObjectiveEvent
{
    /// <summary>
    /// The objective associated to the event
    /// </summary>
    public IChallengeObjective Objective { get; } = objective;
}
