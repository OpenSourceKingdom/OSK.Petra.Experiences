using OSK.Petra.Experiences.Events;

namespace OSK.Petra.Experiences.Models;

/// <summary>
/// Reoresents an objective that reacts to events triggered by an application
/// </summary>
public interface IEventObjective
{
    /// <summary>
    /// Process the event against the provided objective context
    /// </summary>
    /// <param name="context">The context to reference when handling objective processing</param>
    /// <param name="appEvent">The event that triggered this update</param>
    void ProcessEvent(IObjectiveContext context, IApplicationExperienceEvent appEvent);
}
