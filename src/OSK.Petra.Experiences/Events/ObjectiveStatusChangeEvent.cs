using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

/// <summary>
/// An event triggered by the service for status changes with an objective
/// </summary>
/// <param name="context">The context for the experience</param>
/// <param name="challenge">The challenge</param>
/// <param name="objective">The objective</param>
/// <param name="status">The new status fpr the objective</param>
public class ObjectiveStatusChangeEvent(IExperienceContext context, IExperienceChallenge challenge, IChallengeObjective objective, ObjectiveStatus status)
    : ObjectiveEvent(context, challenge, objective)
{
    /// <summary>
    /// The new status fpr the objective
    /// </summary>
    public ObjectiveStatus Status { get; } = status;
}
