using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

public class ExperienceAddedEvent(IExperience experience): ExperienceEvent(experience)
{
}
