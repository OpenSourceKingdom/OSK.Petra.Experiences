using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

/// <summary>
/// An event that provides insights to status changes for an experience
/// </summary>
/// <param name="context">The context for the experience</param>
/// <param name="status">The updated status for the experience</param>
public class ExperienceStatusChangeEvent(IExperienceContext context, ExperienceStatus status): ExperienceEvent(context)
{
    /// <summary>
    /// The updated status for the experience
    /// </summary>
    public ExperienceStatus Status { get; } = status;
}
