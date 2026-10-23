using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

public class ChallengeStatusChangeEvent(IExperience experience, IExperienceChallenge challenge, ChallengeStatus status): ChallengeEvent(experience, challenge)
{
    public ChallengeStatus Status { get; } = status;
}
