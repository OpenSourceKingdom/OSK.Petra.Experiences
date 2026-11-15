using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Events;

/// <summary>
/// An event that provides insights on the pin state changes for an experience
/// </summary>
/// <param name="context">The context for the experience</param>
/// <param name="isPinned">Whether the user has pinned the experience or not</param>
public class ExperiencePinEvent(IExperienceContext context, bool isPinned): ExperienceEvent(context)
{
    /// <summary>
    /// Is the experienced pinned - i.e. tracked by the user
    /// </summary>
    public bool IsPinned { get; } = isPinned;
}
