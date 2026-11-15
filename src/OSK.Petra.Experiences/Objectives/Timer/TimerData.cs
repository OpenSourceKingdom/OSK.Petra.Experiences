using OSK.Petra.Experiences.Models;
using System;

namespace OSK.Petra.Experiences.Objectives.Timer;

/// <summary>
/// Data used by a <see cref="TimerObjective"/>
/// </summary>
public readonly struct TimerData: IObjectiveData
{
    #region Variables

    /// <summary>
    /// The current time for the data
    /// </summary>
    public TimeSpan Value { get; init; }

    #endregion

    #region IObjectiveData

    object IObjectiveData.Value => Value;

    public object Clone()
        => new TimerData()
        {
            Value = Value
        };

    #endregion

    #region Operators

    public override string ToString()
        => Value.ToString();

    public static implicit operator TimeSpan(TimerData state)
        => state.Value;

    public static implicit operator TimerData(TimeSpan timeSpan)
        => new()
        {
            Value = timeSpan
        };

    #endregion
}
