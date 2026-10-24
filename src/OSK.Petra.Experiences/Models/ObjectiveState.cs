using System;

namespace OSK.Petra.Experiences.Models;

public class ObjectiveState
{
    #region Variables

    public required int ObjectiveId { get; init; }

    public ObjectiveStatus Status { get; set; }

    public ICloneable? Data { get; set; }

    #endregion

    #region Api

    public ObjectiveState Clone()
        => new()
        {
            ObjectiveId = ObjectiveId,
            Status = Status,
            Data = (ICloneable?)Data?.Clone()
        };

    #endregion
}
