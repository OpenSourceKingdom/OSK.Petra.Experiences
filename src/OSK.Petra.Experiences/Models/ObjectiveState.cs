namespace OSK.Petra.Experiences.Models;

/// <summary>
/// The state for an objective
/// </summary>
public class ObjectiveState
{
    #region Variables

    /// <summary>
    /// The unique id for the objective
    /// </summary>
    public required string ObjectiveId { get; init; }

    /// <summary>
    /// The current status for the objective
    /// </summary>
    public ObjectiveStatus Status { get; set; }

    /// <summary>
    /// The data the objective state uses
    /// </summary>
    public IObjectiveData? Data { get; set; }

    #endregion

    #region Api

    /// <summary>
    /// Attempts to get the data for the state as the specified type. Will attempt a match against the <see cref="Data"/> value first, falling back to the <see cref="Data"/> as a whole.
    /// </summary>
    /// <typeparam name="TData">The type of data the state is expected to be</typeparam>
    /// <param name="data">The data</param>
    /// <returns>Whether the data stored in state matches the expectations</returns>
    public bool TryGetAs<TData>(out TData? data)
    {
        if (Data is null)
        {
            data = default;
            return false;
        }
        if (Data.Value is TData typedValue)
        {
            data = typedValue;
            return true;
        }
        if (Data is TData typedData)
        {
            data = typedData;
            return true;
        }

        data = default;
        return false;
    }

    /// <summary>
    /// Clones the objective state so that data manipulations do not impact this state object
    /// </summary>
    /// <returns>A cloned state of the current data</returns>
    public ObjectiveState Clone()
        => new()
        {
            ObjectiveId = ObjectiveId,
            Status = Status,
            Data = (IObjectiveData?)Data?.Clone()
        };

    #endregion
}
