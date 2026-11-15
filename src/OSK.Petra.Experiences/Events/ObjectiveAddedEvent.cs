using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

/// <summary>
/// An event that is triggered by the service when a new objective has been added for the user
/// </summary>
/// <param name="context">The context for the experience</param>
/// <param name="challenge">The challenge</param>
/// <param name="objective">The added objective</param>
public class ObjectiveAddedEvent(IExperienceContext context, IExperienceChallenge challenge, IChallengeObjective objective): ObjectiveEvent(context , challenge, objective)
{
}
