using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

public class ExperienceStatusChangeEvent(IExperienceContext context, ExperienceStatus status): ExperienceEvent(context)
{
    public ExperienceStatus Status { get; } = status;
}
