using OSK.Petra.Experiences.Objectives;
using System;

namespace OSK.Petra.Experiences.Internal.Services;

internal class FuncTextFormatter<TParameters, TState>(Func<TParameters, TState, string> formatter): IObjectiveTextFormatter<TParameters, TState>
    where TParameters: ObjectiveParameters<TParameters, TState>
    where TState : ICloneable
{
    #region IObjectiveTextFormatter

    public string Format(TParameters parameters, TState state)
        => formatter(parameters, state);

    #endregion

}
