using OSK.Petra.Experiences.Internal.Services;
using OSK.Petra.Experiences.Models;
using System;

namespace OSK.Petra.Experiences.Objectives;

public abstract class ObjectiveParameters<TParameters, TData>: ObjectiveParameters<TParameters>
    where TParameters: ObjectiveParameters<TParameters, TData>
    where TData : IObjectiveData
{
    #region Variables

    public IObjectiveTextFormatter<TParameters, TData>? ValueFormatter { get; private set; }

    #endregion

    #region Api

    public override IObjectiveData? GetInitialValue()
        => GetInitialData();

    public string GetValueText(TData state)
        => ValueFormatter?.Format((TParameters)this, state) ?? state.ToString();

    public TParameters WithTextFormatter(IObjectiveTextFormatter<TParameters, TData> formatter)
    {
        ValidateFrozen();

        ValueFormatter = formatter;
        return (TParameters)this;
    }

    public TParameters WithTextFormatter(Func<TParameters, TData, string> formatter)
    {
        ValidateFrozen();
        ValueFormatter = new FuncTextFormatter<TParameters, TData>(formatter);
        return (TParameters)this;
    }

    #endregion

    #region Helpers

    protected abstract TData GetInitialData();

    #endregion
}
