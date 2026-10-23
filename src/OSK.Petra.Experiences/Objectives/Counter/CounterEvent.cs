using System.Collections.Generic;
using System.Linq;

namespace OSK.Petra.Experiences.Objectives.Counter;

public class CounterEvent : ICounterEvent
{
    #region Static

    public static CounterEvent For(long delta, IEnumerable<string>? tags = null)
        => new() { Delta = delta, Tags = tags?.Distinct().ToArray() ?? [] };

    public static CounterEvent For(long delta, string targetId, IEnumerable<string>? tags = null)
        => new() { Delta = delta, TargetId = targetId, Tags = tags?.Distinct().ToArray() ?? [] };

    #endregion

    #region ICounterEvent

    public long Delta { get; init; }

    public string? TargetId { get; init; }

    public IReadOnlyCollection<string> Tags { get; init; } = [];

    #endregion
}
