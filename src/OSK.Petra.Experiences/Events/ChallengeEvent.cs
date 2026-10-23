using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

public abstract class ChallengeEvent(IExperience experience, IExperienceChallenge challenge): ExperienceEvent(experience), IChallengeEvent
{
    public IExperienceChallenge Challenge { get; } = challenge;
}
