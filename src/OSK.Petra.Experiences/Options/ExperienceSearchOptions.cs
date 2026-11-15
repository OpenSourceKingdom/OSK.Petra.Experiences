using OSK.Petra.Experiences.Models;
using System.Linq;

namespace OSK.Petra.Experiences.Options;

/// <summary>
/// An option filter for usage with searching for experiences
/// </summary>
public readonly struct ExperienceSearchOptions()
{
    #region Variables

    /// <summary>
    /// If set, the filter expects to have experiences that match the given pin state
    /// </summary>
    public bool? IsPinned { get; init; }

    /// <summary>
    /// A filter of statuses for the search. An empty filter collection should result in all experiences being returned.
    /// </summary>
    public readonly ExperienceStatus[] StatusFilter { get; init; } = [];

    #endregion

    #region Api

    /// <summary>
    /// Creates a search filter that checks only for pinned experiences
    /// </summary>
    /// <returns>The new options with the setting applied</returns>
    public ExperienceSearchOptions WithPinned()
        => new()
        {
            StatusFilter = StatusFilter,
            IsPinned = true
        };

    /// <summary>
    /// Creates a search filter that checks only for unpinned experiences
    /// </summary>
    /// <returns>The new options with the setting applied</returns>
    public ExperienceSearchOptions WithouthPinned()
        => new()
        {
            StatusFilter = StatusFilter,
            IsPinned = false
        };

    /// <summary>
    /// Creates a search filter that includes the provided status
    /// </summary>
    /// <param name="status">The status to check experiences against</param>
    /// <returns>The new options with the setting applied</returns>
    public ExperienceSearchOptions WithStatus(ExperienceStatus status)
        => new()
        {
            StatusFilter = StatusFilter?.Append(status).Distinct().ToArray() ?? [status]
        };

    #endregion
}
