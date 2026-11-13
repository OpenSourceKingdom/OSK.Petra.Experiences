using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

public abstract class ChallengeEvent(IExperienceContext context, IExperienceChallenge challenge): ExperienceEvent(context), IChallengeEvent
{
    public IExperienceChallenge Challenge { get; } = challenge;
}
