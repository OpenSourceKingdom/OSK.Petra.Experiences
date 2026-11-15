namespace OSK.Petra.Experiences.Objectives;

/// <summary>
/// A custom text formatter for an objective
/// </summary>
/// <typeparam name="TParameters">The type of parameters associated with the objective</typeparam>
/// <typeparam name="TData"></typeparam>
public interface IObjectiveTextFormatter<TParameters, TData>
    where TParameters: ObjectiveParameters
{
    /// <summary>
    /// Formats the provided parameters and data into a string for display
    /// </summary>
    /// <param name="parameters">The parameters associated with the objective</param>
    /// <param name="data">The data associated with the objective</param>
    /// <returns>The formatted string</returns>
    string Format(TParameters parameters, TData data);
}
