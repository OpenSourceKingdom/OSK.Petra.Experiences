using OSK.Petra.Experiences.Events;
using System.Collections.Generic;

namespace OSK.Petra.Experiences.Objectives.Counter;

/// <summary>
/// Represents an event that is used for counter style objectives
/// </summary>
public interface ICounterEvent: IApplicationExperienceEvent
{
    /// <summary>
    /// The amount of change for the counter
    /// </summary>
    long Delta { get; }

    /// <summary>
    /// The target this counter is for, if any
    /// </summary>
    string? TargetId { get; }

    /// <summary>
    /// The tags associated with the event
    /// </summary>
    IReadOnlyCollection<string> Tags { get; }
}
