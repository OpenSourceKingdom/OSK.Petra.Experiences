using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

/// <summary>
/// An event that is triggered by the experience service to indicate an experience has been removed for the user. 
/// </summary>
/// <param name="context">The context associated with the experience</param>
public class ExperienceRemovedEvent(IExperienceContext context): ExperienceEvent(context)
{
}
