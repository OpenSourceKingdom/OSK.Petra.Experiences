using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

public class ExperienceStatusChangeEvent(IExperience experience, ExperienceStatus status): ExperienceEvent(experience)
{
    public ExperienceStatus Status { get; } = status;
}
