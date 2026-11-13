using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

public class ObjectiveAddedEvent(IExperienceContext context, IExperienceChallenge challenge, IChallengeObjective objective): ObjectiveEvent(context , challenge, objective)
{
}
