using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Objectives;

/// <summary>
/// A base class for objectives
/// </summary>
/// <typeparam name="TParameters">The type of parameters the objective uses</typeparam>
/// <typeparam name="TData">The type of data that the objective uses</typeparam>
/// <param name="parameters">The parameters for the objective</param>
public abstract class ObjectiveBase<TParameters, TData>(TParameters parameters) : ObjectiveBase<TParameters>(parameters)
    where TParameters : ObjectiveParameters<TParameters, TData>
    where TData: IObjectiveData
{
    #region ObjectiveBase Overrides

    public override string GetValueText(ObjectiveState state)
        => state.Data is TData typedState ? Parameters.GetValueText(typedState) : string.Empty;

    #endregion
}
