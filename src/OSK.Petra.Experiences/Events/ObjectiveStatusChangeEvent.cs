using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

public class ObjectiveStatusChangeEvent(IExperienceContext context, IExperienceChallenge challenge, IChallengeObjective objective, ObjectiveStatus status)
    : ObjectiveEvent(context, challenge, objective)
{
    public ObjectiveStatus Status { get; } = status;
}
