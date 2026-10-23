using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

public class ObjectiveStatusChangeEvent(IExperience experience, IExperienceChallenge challenge, IChallengeObjective objective, ObjectiveStatus status)
    : ObjectiveEvent(experience, challenge, objective)
{
    public ObjectiveStatus Status { get; } = status;
}
