using OSK.Petra.Experiences.Models;
using System.Collections.Generic;

namespace OSK.Petra.Experiences.Options;

/// <summary>
/// A set of options to associate with adding an experience to the service. This is used to provide additional information about the experience, such as its initial state, whether it is pinned, etc.
/// </summary>
public class AddExperienceOptions
{
    /// <summary>
    /// The status to set for the experience. If not provided, the experience will be added with a default status of <see cref="ExperienceStatus.Available"/>.
    /// </summary>
    public ExperienceStatus? Status { get; set; }

    /// <summary>
    /// Whether the experience is pinned
    /// </summary>
    public bool IsPinned { get; set; }

    /// <summary>
    /// The id for an active challenge
    /// </summary>
    public string? ActiveChallengeId { get; set; }

    /// <summary>
    /// The states associated with the experience
    /// </summary>
    public IEnumerable<ChallengeState> ChallengeStates { get; set; } = [];

    /// <summary>
    /// The outputs associated with the experience
    /// </summary>
    public Dictionary<string, object> Outputs { get; set; } = [];
}
