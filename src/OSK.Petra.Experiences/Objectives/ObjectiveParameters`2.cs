using OSK.Petra.Experiences.Internal.Services;
using System;

namespace OSK.Petra.Experiences.Objectives;

public abstract class ObjectiveParameters<TParameters, TState>: ObjectiveParameters<TParameters>
    where TParameters: ObjectiveParameters<TParameters, TState>
    where TState : ICloneable
{
    #region Variables

    public IObjectiveTextFormatter<TParameters, TState>? ValueFormatter { get; private set; }

    #endregion

    #region Api

    public override ICloneable? GetInitialValue()
        => GetInitialState();

    public string GetValueText(TState state)
        => ValueFormatter?.Format((TParameters)this, state) ?? state.ToString();

    public TParameters WithTextFormatter(IObjectiveTextFormatter<TParameters, TState> formatter)
    {
        ValidateFrozen();

        ValueFormatter = formatter;
        return (TParameters)this;
    }

    public TParameters WithTextFormatter(Func<TParameters, TState, string> formatter)
    {
        ValidateFrozen();
        ValueFormatter = new FuncTextFormatter<TParameters, TState>(formatter);
        return (TParameters)this;
    }

    #endregion

    #region Helpers

    protected abstract TState GetInitialState();

    #endregion
}
