using System;

namespace OSK.Petra.Experiences.Models;

public class ObjectiveState
{
    #region Variables

    public required string ObjectiveId { get; init; }

    public ObjectiveStatus Status { get; set; }

    public ICloneable? Data { get; set; }

    #endregion

    #region Api

    public TValue? ValueAs<TValue>()
        => Data is TValue typedValue ? typedValue : throw new InvalidCastException($"Objective state data is not of type '{typeof(TValue).Name}'.");

    public bool TryGetValue<TValue>(out TValue? value)
    {
        if (Data is TValue typedValue)
        {
            value = typedValue;
            return true;
        }

        value = default;
        return false;
    }

    public ObjectiveState Clone()
        => new()
        {
            ObjectiveId = ObjectiveId,
            Status = Status,
            Data = (ICloneable?)Data?.Clone()
        };

    #endregion
}
