using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

public class ExperiencePinEvent(IExperience experience, bool isPinned): ExperienceEvent(experience)
{
    public bool IsPinned { get; } = isPinned;
}
