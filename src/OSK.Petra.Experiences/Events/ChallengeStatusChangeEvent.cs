using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

/// <summary>
/// An event that provides information to the status changes for a challenge
/// </summary>
/// <param name="context">The context for the experience</param>
/// <param name="challenge">The challenge that the event is associated with</param>
/// <param name="status">The updated status</param>
public class ChallengeStatusChangeEvent(IExperienceContext context, IExperienceChallenge challenge, ChallengeStatus status): ChallengeEvent(context, challenge)
{
    /// <summary>
    /// The new status for the challenge
    /// </summary>
    public ChallengeStatus Status { get; } = status;
}
