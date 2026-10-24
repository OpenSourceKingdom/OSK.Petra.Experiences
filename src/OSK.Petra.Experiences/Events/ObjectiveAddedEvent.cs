using OSK.Petra.Experiences.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSK.Petra.Experiences.Events;

public class ObjectiveAddedEvent(IExperience experience, IExperienceChallenge challenge, IChallengeObjective objective): ObjectiveEvent(experience, challenge, objective)
{
}
