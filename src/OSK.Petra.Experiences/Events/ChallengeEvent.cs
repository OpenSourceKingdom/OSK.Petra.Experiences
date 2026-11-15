using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

/// <summary>
/// An event that is triggered by the experience service for experience challenges
/// </summary>
/// <param name="context">The experience context that triggered the event</param>
/// <param name="challenge">The challenge that is being tracked</param>
public abstract class ChallengeEvent(IExperienceContext context, IExperienceChallenge challenge): ExperienceEvent(context), IChallengeEvent
{
    /// <summary>
    /// The active challenge that the event is associated with
    /// </summary>
    public IExperienceChallenge Challenge { get; } = challenge;
}
