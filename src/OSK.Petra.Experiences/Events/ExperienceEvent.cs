using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

public abstract class ExperienceEvent(IExperience experience): IExperienceEvent
{
    public IExperience Experience { get; } = experience;
}
