using OSK.Petra.Experiences.Models;
using System;

namespace OSK.Petra.Experiences.Objectives;

public abstract class ObjectiveBase<TParameters, TState>(TParameters parameters) : ObjectiveBase<TParameters>(parameters)
    where TParameters : ObjectiveParameters<TParameters, TState>
    where TState: ICloneable
{
    #region ObjectiveBase Overrides

    public override string GetValueText(ObjectiveState state)
        => state.Data is TState typedState ? Parameters.GetValueText(typedState) : string.Empty;

    #endregion
}
