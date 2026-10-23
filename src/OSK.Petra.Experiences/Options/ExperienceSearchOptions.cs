using OSK.Petra.Experiences.Models;
using System.Linq;

namespace OSK.Petra.Experiences.Options;

public readonly struct ExperienceSearchOptions()
{
    #region Variables

    public bool? IsPinned { get; init; }

    public readonly ExperienceStatus[] StatusFilter { get; init; } = [];

    #endregion

    #region Api

    public ExperienceSearchOptions WithPinned()
        => new()
        {
            StatusFilter = StatusFilter,
            IsPinned = true
        };

    public ExperienceSearchOptions WithouthPinned()
        => new()
        {
            StatusFilter = StatusFilter,
            IsPinned = false
        };

    public ExperienceSearchOptions WithStatus(ExperienceStatus status)
        => new()
        {
            StatusFilter = StatusFilter?.Append(status).Distinct().ToArray() ?? [status]
        };

    #endregion
}
