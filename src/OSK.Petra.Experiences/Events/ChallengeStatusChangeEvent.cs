using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

public class ChallengeStatusChangeEvent(IExperienceContext context, IExperienceChallenge challenge, ChallengeStatus status): ChallengeEvent(context, challenge)
{
    public ChallengeStatus Status { get; } = status;
}
