using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

public abstract class ExperienceEvent(IExperienceContext context): IExperienceEvent
{
    public IExperienceContext ExperienceContext { get; } = context;
}
