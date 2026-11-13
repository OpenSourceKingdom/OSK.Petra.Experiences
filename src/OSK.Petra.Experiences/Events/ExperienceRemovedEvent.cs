using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

public class ExperienceRemovedEvent(IExperienceContext context): ExperienceEvent(context)
{
}
