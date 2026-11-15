using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

/// <summary>
/// An event triggered by the service when an update has occured for an objective that processed some change
/// </summary>
/// <param name="context">The context for the experience</param>
/// <param name="challenge">The challenge</param>
/// <param name="objective">The objective</param>
public class ObjectiveUpdatedEvent(IExperienceContext context, IExperienceChallenge challenge, IChallengeObjective objective) : ObjectiveEvent(context, challenge, objective)
{
}
