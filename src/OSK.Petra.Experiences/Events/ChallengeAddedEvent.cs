using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

/// <summary>
/// An event that is triggered by the experience service for newly added experience challenges
/// </summary>
/// <param name="context">The experience context that triggered the event</param>
/// <param name="challenge">The challenge that is being tracked</param>
public class ChallengeAddedEvent(IExperienceContext context, IExperienceChallenge challenge): ChallengeEvent(context, challenge)
{
}
