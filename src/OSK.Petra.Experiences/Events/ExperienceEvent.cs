using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

/// <summary>
/// An event that is triggered by the expweiwnce service for experiences
/// </summary>
/// <param name="context">The context associated with the experience</param>
public abstract class ExperienceEvent(IExperienceContext context): IExperienceEvent
{
    /// <summary>
    /// The context associated with the experience
    /// </summary>
    public IExperienceContext ExperienceContext { get; } = context;
}
