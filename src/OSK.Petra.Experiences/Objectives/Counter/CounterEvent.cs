using System.Collections.Generic;
using System.Linq;

namespace OSK.Petra.Experiences.Objectives.Counter;

/// <summary>
/// A counter event implementation
/// </summary>
public class CounterEvent : ICounterEvent
{
    #region Static

    /// <summary>
    /// Creates a counter event that generic to any target
    /// </summary>
    /// <param name="delta">The delta change for the count</param>
    /// <param name="tags">The tags this event includes</param>
    /// <returns>The event</returns>
    public static CounterEvent For(long delta, IEnumerable<string>? tags = null)
        => new() { Delta = delta, Tags = tags?.Distinct().ToArray() ?? [] };

    /// <summary>
    /// Creates a counter event that is specific to a target
    /// </summary>
    /// <param name="delta">The delta change for the count</param>
    /// <param name="targetId">A specific target that the count is for</param>
    /// <param name="tags">The tags this event includes</param>
    /// <returns>The event</returns>
    public static CounterEvent For(long delta, string targetId, IEnumerable<string>? tags = null)
        => new() { Delta = delta, TargetId = targetId, Tags = tags?.Distinct().ToArray() ?? [] };

    #endregion

    #region ICounterEvent

    /// <summary>
    /// The amount of change for the counter
    /// </summary>
    public long Delta { get; init; }

    /// <summary>
    /// The target this count event is associated with
    /// </summary>
    public string? TargetId { get; init; }

    /// <summary>
    /// Any tags that are associated with this event
    /// </summary>
    public IReadOnlyCollection<string> Tags { get; init; } = [];

    #endregion
}
