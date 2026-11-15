using System;
using System.Collections.Generic;
using System.Linq;

namespace OSK.Petra.Experiences.Models;

/// <summary>
/// The state for an experience and its related challenges. This represents the underlying data that is utilized by the experience service and related contexts for data monitoring
/// </summary>
public class ExperienceState
{
    #region Variables

    /// <summary>
    /// The status of the experience
    /// </summary>
    public ExperienceStatus Status { get; set; }

    /// <summary>
    /// Whether the experience is being actively pinned (i.e. UI tracking)
    /// </summary>
    public bool IsPinned { get; set; }

    /// <summary>
    /// The challenge id, if one is currently active
    /// </summary>
    public string? ActiveChallengeId { get; set; }

    /// <summary>
    /// The related challenge states for the experience
    /// </summary>
    public Dictionary<string, ChallengeState> ChallengeStates { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// The outputs lookup for quick reference to stored long-term state
    /// </summary>
    public Dictionary<string, object> Outputs { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    #endregion

    #region Api

    /// <summary>
    /// Gets the active challenge state, if an active challenge is present
    /// </summary>
    public ChallengeState? ActiveChallengeState
        => ActiveChallengeId is not null && ChallengeStates.TryGetValue(ActiveChallengeId, out var state)
            ? state
            : null;

    /// <summary>
    /// Clones the current state data so that data manipulations do not impact this state object
    /// </summary>
    /// <returns>The cloned experience state</returns>
    public ExperienceState Clone()
        => new()
        {
            Status = Status,
            IsPinned = IsPinned,
            ActiveChallengeId = ActiveChallengeId,
            ChallengeStates = ChallengeStates.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Clone(), StringComparer.OrdinalIgnoreCase),
            Outputs = Outputs
        };

    #endregion
}
