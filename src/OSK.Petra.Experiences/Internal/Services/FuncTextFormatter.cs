using OSK.Petra.Experiences.Models;
using OSK.Petra.Experiences.Objectives;
using System;

namespace OSK.Petra.Experiences.Internal.Services;

internal class FuncTextFormatter<TParameters, TData>(Func<TParameters, TData, string> formatter): IObjectiveTextFormatter<TParameters, TData>
    where TParameters: ObjectiveParameters<TParameters, TData>
    where TData : IObjectiveData
{
    #region IObjectiveTextFormatter

    public string Format(TParameters parameters, TData state)
        => formatter(parameters, state);

    #endregion

}
