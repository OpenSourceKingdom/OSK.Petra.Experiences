using System;

namespace OSK.Petra.Experiences.Objectives.Timer;

public readonly struct TimerState: ICloneable
{
    #region Variables

    public TimeSpan Value { get; init; }

    #endregion

    #region ICloneable

    public object Clone()
        => new TimerState()
        {
            Value = Value
        };

    #endregion

    #region Operators

    public override string ToString()
        => Value.ToString();

    public static implicit operator TimeSpan(TimerState state)
        => state.Value;

    public static implicit operator TimerState(TimeSpan timeSpan)
        => new()
        {
            Value = timeSpan
        };

    #endregion
}
