using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

public class ExperienceRemovedEvent(IExperience experience): ExperienceEvent(experience)
{
}
