using System;

namespace OSK.Petra.Experiences.Objectives.Counter;

public readonly struct CounterState : ICloneable
{
    #region Variables

    public long Value { get; init; }

    #endregion

    #region ICloneable

    public object Clone()
        => new CounterState()
        {
            Value = Value
        };

    #endregion

    #region Operators

    public static implicit operator long(CounterState state)
        => state.Value;

    public static implicit operator CounterState(long value)
        => new()
        {
            Value = value
        };

    #endregion
}