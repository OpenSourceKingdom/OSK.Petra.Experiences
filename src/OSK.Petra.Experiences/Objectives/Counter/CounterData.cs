using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Objectives.Counter;

/// <summary>
/// Data for <see cref="CounterData"/>
/// </summary>
public readonly struct CounterData : IObjectiveData
{
    #region Variables

    /// <summary>
    /// The current count
    /// </summary>
    public long Value { get; init; }

    #endregion

    #region IObjectiveData

    object IObjectiveData.Value => Value;

    public object Clone()
        => new CounterData()
        {
            Value = Value
        };

    #endregion

    #region Operators

    public override string ToString()
        => Value.ToString();

    public static implicit operator long(CounterData state)
        => state.Value;

    public static implicit operator CounterData(long value)
        => new()
        {
            Value = value
        };

    #endregion
}