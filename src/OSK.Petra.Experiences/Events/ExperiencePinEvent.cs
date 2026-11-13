using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

public class ExperiencePinEvent(IExperienceContext context, bool isPinned): ExperienceEvent(context)
{
    public bool IsPinned { get; } = isPinned;
}
