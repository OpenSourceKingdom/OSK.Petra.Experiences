using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

/// <summary>
/// An event that is triggered by the experience service to indicate a new experience has been added for the user. This does not mean it is immediately available to start. Please use
/// <see cref="IExperienceContext"/> for more information
/// </summary>
/// <param name="context">The context associated with the experience</param>
public class ExperienceAddedEvent(IExperienceContext context): ExperienceEvent(context)
{
}
