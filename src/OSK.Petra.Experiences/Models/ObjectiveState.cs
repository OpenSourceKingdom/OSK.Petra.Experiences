using System;

namespace OSK.Petra.Experiences.Models;

public class ObjectiveState
{
    #region Variables

    public ObjectiveStatus Status { get; set; }

    public ICloneable? Data { get; set; }

    #endregion

    #region Api

    public ObjectiveState Clone()
        => new()
        {
            Status = Status,
            Data = (ICloneable?)Data?.Clone()
        };

    #endregion
}
