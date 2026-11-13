using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

public class ObjectiveUpdatedEvent(IExperienceContext context, IExperienceChallenge challenge, IChallengeObjective objective) : ObjectiveEvent(context, challenge, objective)
{
}
